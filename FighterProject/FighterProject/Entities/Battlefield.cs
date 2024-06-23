using System;
using FighterProject.Entities;
using FighterProject.Entities.Characters;
using FighterProject.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static FighterProject.Entities.Characters.Character;

namespace FighterProject.Objects {

    /// <summary>
    /// A class representing a Battlefield witch controls interactions
    /// between players during a game.
    /// </summary>
    class Battlefield {

        /// <summary>
        /// The sprite for the Battlefield's background.
        /// </summary>
        public Texture2D BackgroundSprite { get; private set; }

        /// <summary>
        /// The timer.
        /// </summary>
        public Timer Timer { get; private set; }

        // The current players (max 2)
        private readonly Player[] Players = new Player[2];

        // The time for each round.
        private static int _roundTime = 99;
        // True if we have finished the set.
        private bool _isSetDone = false;
        // The timer for the win animation.
        private readonly Timer _setEndTimer = new Timer(3);
        // True if the round ended.
        private bool _roundEnd = false;
        // Counts to a ko.
        private readonly Timer _koTimer = new Timer();
        // The amount of rounds to win to win the set.
        private int _winGoal = 2;
        // The player's starting positions.
        public static Vector2 Player1StartingPos = new Vector2(500, Game1.Floor);
        public static Vector2 Player2StartingPos = new Vector2(1400, Game1.Floor);

        // Toggle DEBUG state.
        private KeyboardState _debugKeyboardState = new KeyboardState();
        private bool _DEBUG = false;
        private bool _debugPressed = false;
        private readonly Keys _debugKey = Keys.F1;

        // Fonts
        private readonly SpriteFont _timerFont;
        private readonly SpriteFont _winFont;

        // Hitstop timer
        private Timer _hitstop = new Timer(0);

        // Background scrolling
        private readonly int _leftCamBound = 0;
        private readonly int _rightCamBound = -960;
        private static readonly int _centerCamBound = -480;
        private readonly float _bgScale = 1.5f;
        private Vector2 _cameraPos = new Vector2(_centerCamBound, -450);

        /// <summary>
        /// A battlefield.
        /// </summary>
        /// <param name="backgroundSprite">The background sprite.</param>
        /// <param name="timerFont">A timer font.</param>
        /// <param name="winFont">A win text font.</param>
        /// <param name="player1">Player 1/</param>
        /// <param name="player2">Player 2.</param>
        public Battlefield(Texture2D backgroundSprite, SpriteFont timerFont, SpriteFont winFont, Player player1, Player player2) {
            BackgroundSprite = backgroundSprite;
            _timerFont = timerFont;
            _winFont = winFont;
            Players[0] = player1;
            Players[1] = player2;
            Players[0].Character.Opponent = Players[1].Character;
            Players[1].Character.Opponent = Players[0].Character;
            Timer = new Timer(_roundTime);
        }
        
        /// <summary>
        /// Updates the entities and objects in the battlefield.
        /// </summary>
        /// <param name="timepassed">The time passed since last frame.</param>
        public void Update(float timepassed) {

            _debugKeyboardState = Keyboard.GetState();
            // Debug Toggle
            if (_debugKeyboardState.IsKeyDown(_debugKey) && !_debugPressed) {
                _DEBUG = !_DEBUG;
                _debugPressed = true;
            }
            else if (_debugKeyboardState.IsKeyUp(_debugKey)) {
                _debugPressed = false;
            }

            // Update the players if there is no hitstop.
            _hitstop.Update(timepassed);
            if (_hitstop.IsFinished) {
                foreach (Player p in Players) {
                    p.Update(timepassed);
                }
            }
            else { return; }

            // Combo tracker.
            foreach(Player p in Players) {
                if (p.Character.Opponent.CurrentState.ID != (int)Actions.Hitstun)
                    p.ComboCount = 0;
            }

            // True if the round is over.
            if (_roundEnd) {
                // Check if someone has obtained the win condition.
                foreach (Player p in Players) {
                    if (p.Wins >= _winGoal) {
                        _isSetDone = true;
                    }
                }

                if (_isSetDone)
                    SetEndState(timepassed);
                else
                    RoundEndState(timepassed);
            }
            else {
                FightState(timepassed);
            }
    
        }

        private void FightState(float timepassed) {
            // Scroll the background
            foreach (Player p in Players) {
                if (TouchingLeftBound(p.Character) && !TouchingRightBound(p.Character.Opponent) && p.Character.Velocity.X < 0) {
                    _cameraPos.X -= p.Character.CurrentHorizontalMovement;
                    p.Character.Opponent.Position.X -= p.Character.CurrentHorizontalMovement;

                    if (_cameraPos.X > _leftCamBound) {
                        _cameraPos.X = _leftCamBound;
                        p.Character.Opponent.Position.X += p.Character.CurrentHorizontalMovement;
                    }
                }
                if (TouchingRightBound(p.Character) && !TouchingLeftBound(p.Character.Opponent) && p.Character.Velocity.X > 0) {
                    _cameraPos.X -= p.Character.CurrentHorizontalMovement;
                    p.Character.Opponent.Position.X -= p.Character.CurrentHorizontalMovement;

                    if (_cameraPos.X < _rightCamBound) {
                        _cameraPos.X = _rightCamBound;
                        p.Character.Opponent.Position.X += p.Character.CurrentHorizontalMovement;
                    }
                }
            }

            // Timer
            Timer.Update(timepassed);

            // Check if a player won
            CheckWin();

            // Player's character hit opponent's character
            foreach (Player p in Players) {
                p.DidHitOpponent = false;
                if (CollisionBox.DidCollide(p.Character.DrawPosition, p.Character.Opponent.DrawPosition, p.Character.Hitbox, p.Character.Opponent.Hurtbox, p.Character.Scale, p.Character.Opponent.Scale)) {
                    // If the hitbox group has already collided before and is still active, don't perform any actions.
                    if (p.HitboxGroupId == p.Character.Hitbox.GroupId)
                        return;

                    p.HitboxGroupId = p.Character.Hitbox.GroupId;
                    p.DidHitOpponent = true;

                    // If we hit with a move with a secondary animation, change to that animation.
                    if (p.Character.CurrentAnimation.SecondaryAnimation != null) {
                        p.Character.ChangeState(new Character_ActionState(p.Character, p.Character.CurrentAnimation.SecondaryAnimation));
                        p.DidHitOpponent = false;
                    }
                    // If the opponent are blocking, block. Otherwise, go to hitstun.
                    else if (p.Character.Opponent.isBlockingHigh || p.Character.Opponent.isBlockingLow) {
                        Block(p.Character.Opponent);
                    }
                    else {
                        TakeDamage(p.Character, p.Character.Opponent, p.Character.Hitbox.Damage);
                        p.ComboCount++;
                        p.Character.Opponent.Hitstun = p.Character.Hitbox.Hitstun;
                    }

                }
                else if (p.Character.Hitbox.Equals(Hitbox.None)) {
                    // Reset the hitbox id one hitbox group has ended collision.
                    p.HitboxGroupId = 0;
                }

                // Manage projectile collisions
                foreach (Projectile j in p.Character.Projectiles) {
                    // Check if projectiles collided with each other.
                    foreach (Projectile k in p.Character.Opponent.Projectiles) {
                        if (CollisionBox.DidCollide(j.DrawPosition, k.DrawPosition, j.Hitbox, k.Hitbox, j.Scale, k.Scale) && j.Active) {
                            j.Active = false;
                            k.Active = false;
                        }
                    }
                    // Check if projectile hit opponent.
                    if (CollisionBox.DidCollide(j.DrawPosition, p.Character.Opponent.DrawPosition, j.Hitbox, p.Character.Opponent.Hurtbox, j.Scale, p.Character.Opponent.Scale) && j.Active) {
                        p.DidHitOpponent = true;
                        j.Active = false;
                        if (p.Character.Opponent.isBlockingHigh || p.Character.Opponent.isBlockingLow) {
                            Block(p.Character.Opponent, false, false);
                        }
                        else {
                            TakeDamage(p.Character, p.Character.Opponent, j.Hitbox.Damage, false, false);
                            p.ComboCount++;
                            p.Character.Opponent.Hitstun = j.Hitbox.Hitstun;
                        }

                    }
                }
            }

            // Modify the player character's state.
            foreach (Player p in Players) {
                if (p.DidHitOpponent) {
                    // If we are blocking, block. Otherwise, go to hitstun.
                    if (p.Character.Opponent.isBlockingHigh || p.Character.Opponent.isBlockingLow)
                        p.Character.Opponent.ChangeState((int)Actions.Block);
                    else
                        p.Character.Opponent.ChangeState((int)Actions.Hitstun);

                }
            }
        }

        private void RoundEndState(float timepassed) {
            _koTimer.Update(timepassed);

            foreach (Player p in Players) {
                if(p.Character.Health <= 0) {
                    if(_koTimer.Time*20 > p.Character.Dizzy) {
                        NextRound();
                    }
                }
            }

            if(_koTimer.Time >= 10) {
                _isSetDone = true;
            }
        }

        // If a player won the game, don't check for win again.
        // Also stop the timer.
        // Anything after this check will not run while the round has ended.
        private void SetEndState(float timepassed) {
            foreach (Player p in Players) {
                p.Character.Velocity.X = 0;
                if (p.DidWin) {
                    if (p.Character.CurrentState.ID != (int)Actions.Victory) {
                        p.Character.ChangeState((int)Actions.Victory);
                    }
                }
            }            

            if (_setEndTimer.IsFinished) { NextRound(); }
            _setEndTimer.Update(timepassed);
        }

        /// <summary>
        /// Draw the Battlefield.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch, Texture2D testSprite) {
            // Background
            spriteBatch.Draw(
                BackgroundSprite,
                _cameraPos,
                new Rectangle(0, 0, Game1.Right_Bound, Game1.Bottom_Bound),
                Color.White,
                0f,
                Vector2.Zero,
                _bgScale,
                SpriteEffects.None,
                0f);

            // Temporary UI
            spriteBatch.Draw(
                testSprite,
                new Vector2(Game1.Center_Bound.X - (Players[0].Character.Health * 4) - 50, 40),
                new Rectangle(0, 0, Players[0].Character.Health * 4, 50),
                Color.Yellow,
                0f,
                Vector2.Zero,
                Game1.Default_Scale,
                SpriteEffects.None,
                0f);
            spriteBatch.Draw(
                testSprite,
                new Vector2(Game1.Center_Bound.X + 30, 40),
                new Rectangle(0, 0, Players[1].Character.Health * 4, 50),
                Color.Yellow,
                0f,
                Vector2.Zero,
                Game1.Default_Scale,
                SpriteEffects.None,
                0f);
            spriteBatch.DrawString(_timerFont, Timer.Time.ToString(), new Vector2(Game1.Center_Bound.X - 20, 40), Color.White);
            // Debug UI
            if (_DEBUG) {
                spriteBatch.Draw(
                    testSprite,
                    new Vector2(Game1.Center_Bound.X - (Players[0].Character.Dizzy) - 50, 140),
                    new Rectangle(0, 0, (int)Math.Round(Players[0].Character.Dizzy), 30),
                    Color.Purple,
                    0f,
                    Vector2.Zero,
                    Game1.Default_Scale,
                    SpriteEffects.None,
                    0f);
                spriteBatch.Draw(
                    testSprite,
                    new Vector2(Game1.Center_Bound.X + 30, 140),
                    new Rectangle(0, 0, (int)Math.Round(Players[1].Character.Dizzy), 30),
                    Color.Purple,
                    0f,
                    Vector2.Zero,
                    Game1.Default_Scale,
                    SpriteEffects.None,
                    0f);
            }

            // Number of wins UI
            for (int i = 0; i < Players[0].Wins; i++) {
                spriteBatch.Draw(
                    testSprite,
                    new Vector2( (Game1.Center_Bound.X - (50 * i)) - 50, 100),
                    new Rectangle(25, 25, 25, 25),
                    Color.Red,
                    0f,
                    Vector2.Zero,
                    Game1.Default_Scale,
                    SpriteEffects.None,
                    0f);
            }
            for (int j = 0; j < Players[1].Wins; j++) {
                spriteBatch.Draw(
                    testSprite,
                    new Vector2(Game1.Center_Bound.X + (50 * j), 100),
                    new Rectangle(25, 25, 25, 25),
                    Color.Blue,
                    0f,
                    Vector2.Zero,
                    Game1.Default_Scale,
                    SpriteEffects.None,
                    0f);
            }

            // Combo Counter
            if (Players[0].ComboCount >= 2)
                spriteBatch.DrawString(_winFont, $"{Players[0].ComboCount} Combo", new Vector2(Game1.Left_Bound + 30, 100), Color.LimeGreen, 0, Vector2.Zero, 2f, SpriteEffects.None, 1f);

            if(Players[1].ComboCount >= 2)
                spriteBatch.DrawString(_winFont, $"{Players[1].ComboCount} Combo", new Vector2(Game1.Right_Bound - 200, 100), Color.LimeGreen, 0, Vector2.Zero, 2f, SpriteEffects.None, 1f);

            // Round win UI
            if (_roundEnd && !_isSetDone) {
                spriteBatch.DrawString(_winFont, _koTimer.Time.ToString(), Game1.Center_Bound - new Vector2(150, 20), Color.Orange, 0, Vector2.Zero, 3f, SpriteEffects.None, 1f);
            }
            // Set end UI
            else if (_isSetDone) {
                // Player 1 win
                if (Players[0].DidWin) {
                    spriteBatch.DrawString(_winFont, "Player 1 Wins!", Game1.Center_Bound - new Vector2(200,20), Color.Red, 0, Vector2.Zero, 3f, SpriteEffects.None, 1f);
                }
                // Player 2 win
                else if (Players[1].DidWin) {
                    spriteBatch.DrawString(_winFont, "Player 2 Wins!", Game1.Center_Bound - new Vector2(200,20), Color.Blue, 0, Vector2.Zero, 3f, SpriteEffects.None, 1f);
                }
                // No Winner 
                else {
                    spriteBatch.DrawString(_winFont, "Draw", Game1.Center_Bound - new Vector2(100, 20), Color.DarkGray, 0, Vector2.Zero, 3f, SpriteEffects.None, 1f);
                }
            }

            // Draw the players and debug
            foreach(Player p in Players) {
                // TEST SPRITE
                if (_DEBUG) 
                    p.Character.DrawDebug(spriteBatch, testSprite);
                
                // Players
                p.Draw(spriteBatch);
            }

        }

        private void CheckWin() {
            // If time runs out, determine who wins.
            if (Timer.IsFinished) {
                // Tie game
                if (Players[0].Character.Health == Players[1].Character.Health) {
                    Stalemate();
                }
                // Player 1 win
                else if (Players[0].Character.Health > Players[1].Character.Health) {
                    PlayerWins(Players[0]);
                    Players[0].DidWin = true;
                }
                // Player 2 win
                else if (Players[0].Character.Health < Players[1].Character.Health) {
                    PlayerWins(Players[1]);
                    Players[1].DidWin = true;
                }
                return;
            }

            // Tie game
            if (Players[0].Character.Health <= 0 && Players[1].Character.Health <= 0) {
                Stalemate();
                return;
            }
            // Player 1 win
            else if (Players[1].Character.Health <= 0) {
                PlayerWins(Players[0]);
                Players[0].DidWin = true;
                return;
            }
            // Player 2 win
            else if (Players[0].Character.Health <= 0) {
                PlayerWins(Players[1]);
                Players[1].DidWin = true;
                return;
            }
        }

        /// <summary>
        /// Checks if enough wins have been gained to win the set.
        /// </summary>
        /// <returns>True if a player reaches the round amount.</returns>
        public bool IsBattlefieldDone() {
            if (!_setEndTimer.IsFinished)
               return false;

            return _isSetDone;
        }

        /// <summary>
        /// Performs logic for when a player wins a round.
        /// </summary>
        /// <param name="player">The player that won.</param>
        private void PlayerWins(Player player) {
            player.Character.Velocity.X = 0;
            player.Character.Opponent.Velocity.X = 0;
            player.Character.Opponent.ChangeState((int)Actions.Defeat);
            player.Wins++;
            _roundEnd = true;
        }

        /// <summary>
        /// Performs logic for when no players win the round.
        /// </summary>
        private void Stalemate() {
            foreach(Player p in Players) {
                p.Character.Velocity.X = 0;
                p.Character.ChangeState((int)Actions.Defeat);
            }
            _roundEnd = true;
        }

        /// <summary>
        /// Deal damage to the character.
        /// </summary>
        /// <param name="attacker">The character attacking.</param>
        /// <param name="defender">The character getting hit.</param>
        /// <param name="damage">The damage to deal to the character.</param>
        /// <param name="pushback">True if attacker moves away from hit character.</param>
        /// <param name="hitstop">True if we add histop.</param>
        private void TakeDamage(Character attacker, Character defender, int damage, bool pushback = true, bool hitstop = true) {
            // Deal damage based on hitbox damage.
            defender.Health -= damage;
            defender.Dizzy += damage*1.5f;
            // Move the characters away from each other.
            // Don't apply knockback to opponent if airborne.
            if (defender.IsFacingRight) {
                defender.Velocity.X = -1.3f;
                if (pushback && !attacker.IsAirborne)
                    attacker.Velocity.X = 1f;
            }
            else {
                defender.Velocity.X = 1.3f;
                if (pushback && !attacker.IsAirborne)
                    attacker.Velocity.X = -1f;
            }
            // Add hitstop.
            if(hitstop)
                _hitstop = new Timer(0.1f);
        }

        /// <summary>
        /// The defending character blocks the attack.
        /// </summary>
        /// <param name="character">The attacking character.</param>
        /// <param name="pushback">True if attacker moves away from hit character.</param>
        private void Block(Character character, bool pushback = true, bool hitstop = true) {
            // Move the characters away from each other.
            if (character.IsFacingRight) {
                character.Velocity.X = -0.5f;
                if(pushback && !character.Opponent.IsAirborne)
                    character.Opponent.Velocity.X = 1f;
            }
            else {
                character.Velocity.X = 0.5f;
                if(pushback && !character.Opponent.IsAirborne)
                    character.Opponent.Velocity.X = -1f;
            }
            // Add hitstop.
            if(hitstop)
                _hitstop = new Timer(0.05f);
        }

        /// <summary>
        /// Resets the game to start a new round.
        /// </summary>
        public void NextRound() {
            foreach(Player p in Players) {
                p.Reset();
                p.DidWin = false;
            }
            _roundEnd = false;
            _cameraPos = new Vector2(_centerCamBound, -450);
            _setEndTimer.Reset();
            _koTimer.Reset();
        }

        /// <summary>
        /// Checks if the character is touching left bound.
        /// </summary>
        /// <param name="c">The character.</param>
        /// <returns>True if the character is touching the left wall.</returns>
        private bool TouchingLeftBound(Character c) {
            return c.Position.X - c.BodyBox.Width / 2 <= Game1.Left_Bound;
        }
        /// <summary>
        /// Checks if the characere is touching the right bound.
        /// </summary>
        /// <param name="c">The character.</param>
        /// <returns>True if the character is touching the right wall.</returns>
        private bool TouchingRightBound(Character c) {
            return c.Position.X + c.BodyBox.Width / 2 >= Game1.Right_Bound;
        }

    }
}

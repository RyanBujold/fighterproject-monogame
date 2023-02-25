using FighterProject.Entities;
using FighterProject.GameStates;
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
        /// The first Player.
        /// </summary>
        public Player Player1 { get; private set; }

        /// <summary>
        /// The second Player.
        /// </summary>
        public Player Player2 { get; private set; }

        /// <summary>
        /// The timer.
        /// </summary>
        public Timer Timer { get; private set; }

        // The time for each round
        public static int RoundTime = 60;
        private Timer _roundWinTimer = new Timer(3);
        // True if the round ended.
        private bool _roundEnd = false;
        private bool _player1Win = false;
        private bool _player2Win = false;
        // The player's starting positions.
        public static Vector2 Player1StartingPos = new Vector2(500, Game1.Floor);
        public static Vector2 Player2StartingPos = new Vector2(1400, Game1.Floor);

        // Toggle DEBUG state.
        private KeyboardState _debugKeyboardState = new KeyboardState();
        private bool _DEBUG = false;
        private bool _debugPressed = false;
        private Keys _debugKey = Keys.F1;

        // Fonts
        private SpriteFont _timerFont;
        private SpriteFont _winFont;

        // Collision Helpers.
        private int _p1HitboxGroupId = 0;
        private int _p2HitboxGroupId = 0;

        // Hitstop timer
        private Timer _hitstop = new Timer(0);

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
            Player1 = player1;
            Player2 = player2;
            Player1.Character.Opponent = Player2.Character;
            Player2.Character.Opponent = Player1.Character;
            Timer = new Timer(RoundTime);
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
            if (_hitstop.IsFinished || _roundEnd) {
                Player1.Update(timepassed);
                Player2.Update(timepassed);
            }
            else { return; }

            // Combo tracker.
            if (Player2.Character.CurrentState.ID != (int)Actions.Hitstun)
                Player1.ComboCount = 0;

            if (Player1.Character.CurrentState.ID != (int)Actions.Hitstun)
                Player2.ComboCount = 0;

            // If a player won the game, don't check for win again.
            // Also stop the timer.
            // Anything after this check will not run while the round has ended.
            if (_roundEnd) {
                _roundWinTimer.Update(timepassed);
                if (_roundWinTimer.IsFinished)
                    Reset();
                return;
            }

            // Timer
            Timer.Update(timepassed);

            // If time runs out, determine who wins.
            if (Timer.IsFinished) {
                // Tie game
                if (Player1.Character.Health == Player2.Character.Health) {
                    Stalemate();
                }
                // Player 1 win
                else if (Player1.Character.Health > Player2.Character.Health) {
                    PlayerWins(Player1);
                    _player1Win = true;
                }
                // Player 2 win
                else if (Player1.Character.Health < Player2.Character.Health) {
                    PlayerWins(Player2);
                    _player2Win = true;
                }
                return;
            }

            // Tie game
            if (Player1.Character.Health <= 0 && Player2.Character.Health <= 0) {
                Stalemate();
                return;
            }
            // Player 1 win
            else if (Player2.Character.Health <= 0) {
                PlayerWins(Player1);
                _player1Win = true;
                return;
            }
            // Player 2 win
            else if (Player1.Character.Health <= 0) {
                PlayerWins(Player2);
                _player2Win = true;
                return;
            }
                
            // Check for collision.       
            bool player1Hitplayer2 = false;
            bool player2Hitplayer1 = false;
            // Player 1 hit Player 2
            if (CollisionBox.DidCollide(Player1.Character.DrawPosition, Player2.Character.DrawPosition, Player1.Character.Hitbox, Player2.Character.Hurtbox, Player1.Character.Scale, Player2.Character.Scale)) {
                // If the hitbox group has already collided before and is still active, don't perform any actions.
                if (_p1HitboxGroupId == Player1.Character.Hitbox.GroupId)
                    return;

                _p1HitboxGroupId = Player1.Character.Hitbox.GroupId;
                player1Hitplayer2 = true;
                // If we are blocking, block. Otherwise, go to hitstun.
                if (Player2.Character.Hurtbox.BlockState == Hurtbox.DefenseState.Blocking)
                    Block(Player2);
                else
                    TakeDamage(Player1, Player2, Player1.Character.Hitbox.Damage);

            }
            else if(Player1.Character.Hitbox.Equals(Hitbox.None)) {
                // Reset the hitbox id one hitbox group has ended collision.
                _p1HitboxGroupId = 0;
            }
            // Player 2 hit Player 1
            if (CollisionBox.DidCollide(Player2.Character.DrawPosition, Player1.Character.DrawPosition, Player2.Character.Hitbox, Player1.Character.Hurtbox, Player2.Character.Scale, Player1.Character.Scale)) {
                // If the hitbox group has already collided before and is still active, don't perform any actions
                if (_p2HitboxGroupId == Player2.Character.Hitbox.GroupId)
                    return;

                _p2HitboxGroupId = Player2.Character.Hitbox.GroupId;
                player2Hitplayer1 = true;
                // If we are blocking, block. Otherwise, go to hitstun.
                if (Player1.Character.Hurtbox.BlockState == Hurtbox.DefenseState.Blocking)
                    Block(Player1);
                else
                    TakeDamage(Player2, Player1, Player2.Character.Hitbox.Damage);

            }
            else if(Player2.Character.Hitbox.Equals(Hitbox.None)) {
                // Reset the hitbox id one hitbox group has ended collision.
                _p2HitboxGroupId = 0;
            }
            // Modify the player character's state.
            if (player1Hitplayer2) {
                // If we are blocking, block. Otherwise, go to hitstun.
                if (Player2.Character.Hurtbox.BlockState == Hurtbox.DefenseState.Blocking)
                    Player2.Character.ChangeState((int)Actions.Block);
                else
                    Player2.Character.ChangeState((int)Actions.Hitstun);

            }
            if (player2Hitplayer1) {
                // If we are blocking, block. Otherwise, go to hitstun.
                if (Player1.Character.Hurtbox.BlockState == Hurtbox.DefenseState.Blocking)
                    Player1.Character.ChangeState((int)Actions.Block);
                else
                    Player1.Character.ChangeState((int)Actions.Hitstun);

            }
            // Manage projectile collisions
            // Player 1 projectiles hit Player 2
            foreach(Projectile projectile in Player1.Character.Projectiles) {
                foreach(Projectile projectile2 in Player2.Character.Projectiles) {
                    if(CollisionBox.DidCollide(projectile.DrawPosition, projectile2.DrawPosition, projectile.Hitbox, projectile2.Hitbox, projectile.Scale, projectile2.Scale) && projectile.Active) {
                        projectile.Active = false;
                        projectile2.Active = false;
                    }
                }
                if (CollisionBox.DidCollide(projectile.DrawPosition, Player2.Character.DrawPosition, projectile.Hitbox, Player2.Character.Hurtbox, projectile.Scale, Player2.Character.Scale) && projectile.Active) {
                    projectile.Active = false;
                    if (Player2.Character.Hurtbox.BlockState == Hurtbox.DefenseState.Blocking)
                        Block(Player2, false, false);
                    else
                        TakeDamage(Player1, Player2, projectile.Hitbox.Damage, false, false);

                    if (Player2.Character.Hurtbox.BlockState == Hurtbox.DefenseState.Blocking)
                        Player2.Character.ChangeState((int)Actions.Block);
                    else 
                        Player2.Character.ChangeState((int)Actions.Hitstun);

                }
            }
            // Player 2 projectiles hit Player 1
            foreach (Projectile projectile in Player2.Character.Projectiles) {
                foreach (Projectile projectile2 in Player1.Character.Projectiles) {
                    if (CollisionBox.DidCollide(projectile.DrawPosition, projectile2.DrawPosition, projectile.Hitbox, projectile2.Hitbox, projectile.Scale, projectile2.Scale) && projectile.Active) {
                        projectile.Active = false;
                        projectile2.Active = false;
                    }
                }
                if (CollisionBox.DidCollide(projectile.DrawPosition, Player1.Character.DrawPosition, projectile.Hitbox, Player1.Character.Hurtbox, projectile.Scale, Player1.Character.Scale) && projectile.Active) {
                    projectile.Active = false;
                    if (Player1.Character.Hurtbox.BlockState == Hurtbox.DefenseState.Blocking)
                        Block(Player1, false);
                    else
                        TakeDamage(Player2, Player1, projectile.Hitbox.Damage, false);

                    if (Player1.Character.Hurtbox.BlockState == Hurtbox.DefenseState.Blocking)
                        Player1.Character.ChangeState((int)Actions.Block);
                    else
                        Player1.Character.ChangeState((int)Actions.Hitstun);

                }
            }

        }

        /// <summary>
        /// Draw the Battlefield.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch, Texture2D testSprite) {
            // Background
            spriteBatch.Draw(BackgroundSprite, Vector2.Zero, Color.White);

            // Temporary UI
            spriteBatch.Draw(
                testSprite,
                new Vector2(Game1.Center_Bound.X - (Player1.Character.Health * 4) - 50, 40),
                new Rectangle(0, 0, Player1.Character.Health * 4, 50),
                Color.Yellow,
                0f,
                Vector2.Zero,
                1,
                SpriteEffects.None,
                0f);
            spriteBatch.Draw(
                testSprite,
                new Vector2(Game1.Center_Bound.X + 30, 40),
                new Rectangle(0, 0, Player2.Character.Health * 4, 50),
                Color.Yellow,
                0f,
                Vector2.Zero,
                1,
                SpriteEffects.None,
                0f);
            spriteBatch.DrawString(_timerFont, Timer.Time.ToString(), new Vector2(Game1.Center_Bound.X - 20, 40), Color.White);

            // Number of wins UI
            for (int i = 0; i < Player1.Wins; i++) {
                spriteBatch.Draw(
                    testSprite,
                    new Vector2( (Game1.Center_Bound.X - (50 * i)) - 50, 100),
                    new Rectangle(25, 25, 25, 25),
                    Color.Red,
                    0f,
                    Vector2.Zero,
                    1,
                    SpriteEffects.None,
                    0f);
            }
            for (int j = 0; j < Player2.Wins; j++) {
                spriteBatch.Draw(
                    testSprite,
                    new Vector2(Game1.Center_Bound.X + (50 * j), 100),
                    new Rectangle(25, 25, 25, 25),
                    Color.Blue,
                    0f,
                    Vector2.Zero,
                    1,
                    SpriteEffects.None,
                    0f);
            }

            // Combo Counter
            if(Player1.ComboCount >= 2)
                spriteBatch.DrawString(_winFont, $"{Player1.ComboCount} Combo", new Vector2(Game1.Left_Bound + 30, 100), Color.LimeGreen, 0, Vector2.Zero, 2f, SpriteEffects.None, 1f);

            if(Player2.ComboCount >= 2)
                spriteBatch.DrawString(_winFont, $"{Player2.ComboCount} Combo", new Vector2(Game1.Right_Bound - 200, 100), Color.LimeGreen, 0, Vector2.Zero, 2f, SpriteEffects.None, 1f);

            // Round win UI
            if (_roundEnd) {
                // Player 1 win
                if (_player1Win) {
                    spriteBatch.DrawString(_winFont, "Player 1 Wins!", Game1.Center_Bound - new Vector2(200,20), Color.Red, 0, Vector2.Zero, 3f, SpriteEffects.None, 1f);
                }
                // Player 2 win
                else if (_player2Win) {
                    spriteBatch.DrawString(_winFont, "Player 2 Wins!", Game1.Center_Bound - new Vector2(200,20), Color.Blue, 0, Vector2.Zero, 3f, SpriteEffects.None, 1f);
                }
                // No Winner 
                else {
                    spriteBatch.DrawString(_winFont, "Draw", Game1.Center_Bound - new Vector2(100, 20), Color.DarkGray, 0, Vector2.Zero, 3f, SpriteEffects.None, 1f);
                }
            }

            // TEST SPRITE
            if (_DEBUG) {
                Player1.Character.DrawDebug(spriteBatch, testSprite);
                Player2.Character.DrawDebug(spriteBatch, testSprite);
            }

            // Players
            Player1.Draw(spriteBatch);
            Player2.Draw(spriteBatch, isPlayer2:true);
        }

        /// <summary>
        /// Performs logic for when a player wins a round.
        /// </summary>
        /// <param name="player">The player that won.</param>
        private void PlayerWins(Player player) {
            player.Character.Velocity.X = 0;
            player.Character.Opponent.Velocity.X = 0;
            player.Character.ChangeState((int)Actions.Victory);
            player.Character.Opponent.ChangeState((int)Actions.Defeat);
            player.Wins++;
            _roundEnd = true;
        }

        /// <summary>
        /// Performs logic for when no players win the round.
        /// </summary>
        private void Stalemate() {
            Player1.Character.Velocity.X = 0;
            Player2.Character.Velocity.X = 0;
            Player1.Character.ChangeState((int)Actions.Defeat);
            Player2.Character.ChangeState((int)Actions.Defeat);
            _roundEnd = true;
        }

        /// <summary>
        /// Deal damage to the player.
        /// </summary>
        /// <param name="attacker">The player attacking.</param>
        /// <param name="defender">The player getting hit.</param>
        /// <param name="damage">The damage to deal to the player.</param>
        /// <param name="pushback">True if attacker moves away from hit player.</param>
        /// <param name="hitstop">True if we add histop.</param>
        private void TakeDamage(Player attacker, Player defender, int damage, bool pushback = true, bool hitstop = true) {
            // Deal damage based on hitbox damage.
            defender.Character.Health -= damage;
            // Move the characters away from each other.
            // Don't apply knockback to opponent if airborne.
            if (defender.Character.IsFacingRight) {
                defender.Character.Velocity.X = -1.3f;
                if (pushback && !attacker.Character.IsAirborne)
                    attacker.Character.Velocity.X = 1f;
            }
            else {
                defender.Character.Velocity.X = 1.3f;
                if (pushback && !attacker.Character.IsAirborne)
                    attacker.Character.Velocity.X = -1f;
            }
            // Add hitstop.
            if(hitstop)
                _hitstop = new Timer(0.1f);

            // Combo
            attacker.ComboCount++;
        }

        /// <summary>
        /// The defending player blocks the attack.
        /// </summary>
        /// <param name="player">The attacking player.</param>
        /// <param name="pushback">True if attacker moves away from hit player.</param>
        private void Block(Player player, bool pushback = true, bool hitstop = true) {
            // Move the characters away from each other.
            if (player.Character.IsFacingRight) {
                player.Character.Velocity.X = -0.5f;
                if(pushback && !player.Character.Opponent.IsAirborne)
                    player.Character.Opponent.Velocity.X = 1f;
            }
            else {
                player.Character.Velocity.X = 0.5f;
                if(pushback && !player.Character.Opponent.IsAirborne)
                    player.Character.Opponent.Velocity.X = -1f;
            }
            // Add hitstop.
            if(hitstop)
                _hitstop = new Timer(0.05f);
        }

        /// <summary>
        /// Resets the game to start a new round.
        /// </summary>
        public void Reset() {
            Player1.Reset();
            Player2.Reset(isPlayer2: true);
            Timer.Reset();
            _roundEnd = false;
            _player1Win = false;
            _player2Win = false;
            _roundWinTimer.Reset();
        }

    }
}

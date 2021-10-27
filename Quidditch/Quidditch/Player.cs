using System;
using System.Collections.Generic;
using System.Linq;

namespace Quidditch
{
    public abstract class Player :  Entity
    {
        public int Team;
    }

    public class Keeper : Player   //Gardien
    {
        private const int MaxDistanceToGive = 4;
        public bool Quaffle;
        
        // Initializes the attibutes of the keeper
        /*         - Keeper stands in its ring
         *
         * Tips: You should need to look where the rings are in the field in the file Prints.cs in order to know where are the keepers
         */
        public Keeper(int team, Game game)
        {
            this.Team = team;
            this.ToChar = 'K';
            this.Color =this.Team == 0? ConsoleColor.Green:ConsoleColor.Red;
            Quaffle = false;
            //TODO
            if (this.Team == 0) SetXY(3,game.GetFieldHeight() / 2);
            else SetXY(game.GetFieldWidth() - 4, game.GetFieldHeight() / 2);
        }

        // Update the keeper
        public override void Update(Game game)
        {

            if (Quaffle)
            {
                foreach (Chaser p in game.players.Where(x => x.Team == this.Team&& x is Chaser))
                {
                    if(Distance(this,p)<MaxDistanceToGive)
                    {
                        p.Quaffle = true;
                        game.balls.quaffle.taker = p;
                        this.Quaffle = false;
                        return;
                    }

                }
            }
        }
    }
    public class Seeker : Player   //Attrapeur
    {        
        private const int MaxRandomProbabilityToCatch = 75;
        // Initializes the Seeker attributes
        /*         - X and Y are random in the field
         */
        public Seeker(int team, Game game)
        {
            this.Team = team;
            this.ToChar = 'S';
            this.Color =this.Team == 0? ConsoleColor.Green:ConsoleColor.Red;
            if (this.Team == 0)
                SetXY(game._random.Next(1, game.GetFieldWidth() / 2-1), game._random.Next(1, game.GetFieldHeight()));
            else
                SetXY(game._random.Next(game.GetFieldWidth() / 2,game.GetFieldWidth()), game._random.Next(game.GetFieldHeight()));
        }

        // Update the Seeker
        /* Tips: Seeker can see the GoldenSnitch when it is at distance < 8
         * Tipsbis: Seeker can try to catch GoldenSnitch when it is at less then 1 in distance
         * TipsTris: You can get the GoldenSnitch from the game object
         */
        public override void Update(Game game)
        {
            GoldenSnitch goldenSnitch = game.balls.goldenSnitch;
            //move
            if (Distance(this, goldenSnitch) < 8) this.MoveTo(game, goldenSnitch);
            else this.Move(game);
            
            //catch
            if (Distance(this, goldenSnitch) <= 1)
            {
                int prob = game._random.Next(MaxRandomProbabilityToCatch);
                if (prob == game._random.Next(MaxRandomProbabilityToCatch))
                {
                    game.balls.goldenSnitch.Taken = true;
                    game.score[this.Team] += 150;
                }
            }
            
        }
        
        
    }
    public class Beater : Player   //Batteur
    {
        // Initializes Beater's attributes
        /*         - X and Y are random positions in the field
         */
        public Beater(int team, Game game)
        {
            this.Team = team;
            this.ToChar = 'B';
            this.Color =this.Team == 0? ConsoleColor.Green:ConsoleColor.Red;
            if (this.Team == 0)
                SetXY(game._random.Next(1, game.GetFieldWidth() / 2-1), game._random.Next(1, game.GetFieldHeight()));
            else
                SetXY(game._random.Next(game.GetFieldWidth() / 2,game.GetFieldWidth()), game._random.Next(game.GetFieldHeight()));
        }

        // Update the Beater
        public override void Update(Game game)
        {
            game.balls.bludgers.ForEach(x => {this.MoveTo(game, x);});
            
        }
    }
    public class Chaser : Player   //Poursuiveur
    {
        private const int MaxDistanceCoChaser = 4;
        private const int MaxRandomToScore = 3;
        public bool Quaffle;
        public int Ko;
        
        // Initialize chaser attributes
        /*         - X and Y are random positions in the field
         */
        public Chaser(int team, Game game)
        {
            this.Team = team;
            this.ToChar = 'C';
            this.Color =this.Team == 0? ConsoleColor.Green:ConsoleColor.Red;
            Quaffle = false;
            Ko = 0;
            if (this.Team == 0)
                SetXY(game._random.Next(1, game.GetFieldWidth() / 2-1), game._random.Next(1, game.GetFieldHeight()));
            else
                SetXY(game._random.Next(game.GetFieldWidth() / 2,game.GetFieldWidth()), game._random.Next(game.GetFieldHeight()));
        }
        
        // HaveQuaffleAndGoToKeeper:
        /* Tips: 'is' can be a useful key word
         */
        private void HaveQuaffleAndGoToKeeper(Game game, Keeper keeper)
        {
            MoveTo(game,keeper);
            foreach (Chaser chaser in game.players.Where(x => x is Chaser))
            {
                if (Distance(chaser, this) < MaxDistanceCoChaser && Distance(chaser, keeper) < Distance(keeper, this))
                {
                    chaser.Quaffle = true;
                    game.balls.quaffle.taker = chaser;
                    this.Quaffle = false;
                    return;
                }
            }
        }

        // HaveQuaffleAndTryToScore
        private void HaveQuaffleAndTryToScore(Game game, Keeper keeper)
        {
            int prob = game._random.Next(MaxRandomToScore);
            if (prob == game._random.Next(MaxRandomToScore))
            {
                game.score[Team] += 10;
            }

            this.Quaffle = false;
            game.balls.quaffle.taker = keeper;
            keeper.Quaffle = true;
        }

        // DoNotHaveQuaffle
        private void DoNotHaveQuaffle(Game game, Keeper keeper, Quaffle quaffle)
        {
            Player p = (Player)quaffle.taker;
            if (p == null) return;
            if (p.Team == this.Team && p is Keeper)
            {
                this.MoveTo(game,p);
            }
            if(p.Team==this.Team)
            {
                this.Move(game);
                this.Move(game);
                this.MoveTo(game,keeper);
            }
            else
            {
                this.MoveTo(game,p);
            }
          
        }

        // QuaffleNotTaken
        private void QuaffleNotTaken(Game game, Quaffle quaffle)
        {
            this.MoveTo(game,quaffle);
            if (Distance(quaffle, this) == 0)
            {
                quaffle.taker = this;
                this.Quaffle = true;
            }
        }
        
        // Update the Chaser:
        /* Tips : Chaser can score when the distance from the keeper is less than 4
         * Tipsbis : You should use the function GiveMeKeeper
         */
        public override void Update(Game game)
        {
            if (this.Ko>0)
            {
                this.Ko -= 1;
                if (this.Quaffle)
                {
                    this.Quaffle = true;
                    game.balls.quaffle.taker = null;
                }
            }
            else
            {
                if (this.Quaffle)
                {
                    Keeper k = GiveMeKeeper(game);
                    if (Distance(k, this) < 4)
                    {
                        HaveQuaffleAndTryToScore(game, k);
                        return;
                    }

                    HaveQuaffleAndGoToKeeper(game, k);

                }
                else
                {
                    if (game.balls.quaffle.taker == null)
                    {
                        this.QuaffleNotTaken(game,game.balls.quaffle);
                        return;
                    }
                    DoNotHaveQuaffle(game,GiveMeKeeper(game),game.balls.quaffle);
                }
            }
            
            
        }
        
        
        // This function returns the keeper of the opposite team
        private Keeper GiveMeKeeper(Game game)
        {
            foreach (var player in game.players)
            {
                if (player is Keeper && player.Team != Team)
                    return (Keeper) player;
            }
            return null; //This will never appen
        }
    }
}

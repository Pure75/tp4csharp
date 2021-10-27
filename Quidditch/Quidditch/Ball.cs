using System;
using System.Collections.Generic;
using System.Linq;

namespace Quidditch
{
    
    public class Quaffle : Entity
    {
        public Entity taker;
        
        // Initialize Quaffle's fields
        /*         - the position
         *         - the color -> DarkYellow
         *         - the representation
         */
        public Quaffle(int x, int y)
        {
            this.ToChar = 'Q';
            this.Color = ConsoleColor.DarkYellow;
            this.SetXY(x,y);
        }

        // Update the Quaffle
        /* Tips: The Quaffle has the position of his possessor
         */
        public override void Update(Game game)
        {
            if (this.taker!=null)
            {
                if (taker is Chaser) if (((Chaser) taker).Quaffle==false)
                {
                    this.taker = null;
                    return;
                }
                if (taker is Keeper) if (((Keeper) taker).Quaffle==false){
                    this.taker = null;
                    return;
                }
                SetXY(taker.X,taker.Y);
                
            }
            
        }
    }
    
    public class Bludger : Entity
    {
        private const int DistanceAttack = 2;
        private const int DistanceBeater = 3;
        private const int MaxHit = 8;
        private const int MinHit = 4;
        private const int MaxRandomChanceToHit = 10;
        private const int MaxChanceToHit = 3;
        
        // Initialize bludger's field
        /*        - the position
         *        - the color -> DarkGray
         *        - the representation
         */
        public Bludger(int x, int y)
        {
            this.ToChar = 'B';
            this.Color = ConsoleColor.DarkGray;
            this.SetXY(x,y);
        }
        
        // CanAttack tells if the bludger can attack a player
        /* Tips: Look for the special word 'is'
         */
        public bool CanAttack(Player player, Game game)
        {
            if (!(player is Chaser))
                return false;

            if (((Chaser) player).Ko != 0)
                return false;

            if (Distance(player, this) >= DistanceAttack)
                return false;

            List<Player> players=game.players.Where(x => x.Team == player.Team&& x is Beater).ToList();
            foreach (Player p in players)
            {
                if (Distance(player, p) < DistanceBeater)
                    return false;

            }
           
            return true;
        }
        
        // Update the bludger
        /* Tips: Game has a field Balls that has a field Quaffle
         */
        public override void Update(Game game)
        {

            if (game.balls.quaffle.taker!=null)
            {
                this.MoveTo(game,game.balls.quaffle.taker);
                this.MoveTo(game,game.balls.quaffle.taker);
            }
            this.Move(game);
            foreach (Chaser player in game.players.Where(x => x is Chaser))
            {
                if (CanAttack(player, game))
                {
                    int prob = game._random.Next(MaxChanceToHit / MaxRandomChanceToHit);
                    if (prob == game._random.Next(MaxChanceToHit / MaxRandomChanceToHit))
                        player.Ko = game._random.Next(MinHit, MaxHit);
                }
            }
        }
    }
    
    public class GoldenSnitch : Entity
    {
        public bool Taken = false;
        
        // Initialize GoldenSnitch's fields
        /*         - the position
         *         - the color -> Yellow
         *         - the representation
         */
        public GoldenSnitch(int x, int y)
        {
            this.ToChar = 'G';
            this.SetXY(x,y);
            this.Color = ConsoleColor.Yellow;

        }

        // Update the GoldenSnitch
        public override void Update(Game game)
        {
            for (int i = 0; i < 5; i++)
            {
                this.Move(game);
            }
        }
    }
    
    // This class gives fast access to every balls of the game
    public class Balls
    {
        public List<Bludger> bludgers;
        public GoldenSnitch goldenSnitch;
        public Quaffle quaffle;

        public List<Entity> all;
        public Balls(Game game)
        {
            int h = game.GetFieldHeight();
            int w = game.GetFieldWidth();
            quaffle = new Quaffle(w / 2, h / 2);
            bludgers = new List<Bludger>();
            for (int i = 0; i < 2; i++)
                bludgers.Add(new Bludger(game._random.Next(w),game._random.Next(h)));
            goldenSnitch = new GoldenSnitch(game._random.Next(w),game._random.Next(h));
            
            all = new List<Entity>();
            all.Add(quaffle);
            all.Add(bludgers[0]);
            all.Add(bludgers[1]);
            all.Add(goldenSnitch);
        }
    }
}

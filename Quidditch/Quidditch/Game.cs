using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Quidditch
{
    public class Game
    {
        public Balls balls;
        public List<Player> players;
        public int[] score;
        public int time;

        private Printer printer = new Printer();
        
        private int _fieldwidth;
        public int GetFieldWidth()
        {
            return _fieldwidth;
        }
        private int _fieldheight;
        public int GetFieldHeight()
        {
            return _fieldheight;
        }
        public Random _random = new Random();

        public void SetFieldWidth(int w)
        {
            _fieldwidth = w;
        }
        public void SetFieldHeight(int h)
        {
            _fieldheight = h;
        }
        
        
        // AddTeam
        public void AddTeam(int team)
        {
            for (int i = 0; i < 3; i++) players.Add(new Chaser(team, this));
            for (int i = 0; i < 2; i++) players.Add(new Beater(team, this));
            players.Add(new Keeper(team, this));
            players.Add(new Seeker(team, this));

        }

        // Game initializes the game
        /* Tips: To change the name of your Console, (cf. TP1)
         */
        public Game(int w, int h)
        {
            time = 0;
            this._fieldheight = h;
            this._fieldwidth = w;
            this.score = new int[2] {0, 0};
            this.balls = new Balls(this);
            players = new List<Player>();
            this.AddTeam(0);
            this.AddTeam(1);
            Console.Title = "Quidditch Game";
        }

        // ValidPosition
        public bool ValidPosition(int x, int y)
        {

            if (x < 1 || y < 1 || y >= this._fieldheight-1 || x >= this._fieldwidth-1)
                return false;
            if (players.Any(p => p.X == x && p.Y == y))
                return false;
            /*//check if in the left ring
            if (x >= 2 && x <= 2 + 3 && y>=this.GetFieldHeight() / 2 - 1 && y<=this.GetFieldHeight()+3)
                return false;
            // check if in the right ring
            if (x >= this.GetFieldWidth()-5 && x <= this.GetFieldWidth()-5 + 3 && y>=this.GetFieldHeight() / 2 - 1 && y<=this.GetFieldHeight()+3)
                return false;*/
            return true;
        }

        // Update
        /* Tips: Use the function Thread.Sleep(int) to have a break at each step and to be able to see your printing 
         */
        public int Play()
        {
            List<Entity> entities = new List<Entity>();
            entities.AddRange(balls.all);
            entities.AddRange(players);
            while (true)
            {
                if (this.balls.goldenSnitch.Taken)
                    return Winner();
                
                entities.ForEach(e=>e.Update(this));
                this.time += 1;
                printer.PrintGame(this);
                System.Threading.Thread.Sleep(100);

            }
        }

        private int Winner()
        {
            if (this.score[0] >= 150) return 0;
            return 1;
        }
    }
}

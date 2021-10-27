using System;

namespace Quidditch
{
    public abstract class Entity
    {
        // Attributes
        public int X; // X position on the field
        public int Y; // Y position on the field
        public char ToChar; // Representation of the entity
        public ConsoleColor Color; // Color of the representation
        // Initalize a random object
        protected static Random _random = new Random();
        
        // Methods
        // Each entity needs a fuction that updates
        public abstract void Update(Game game);
        
        // Set position
        protected void SetXY(int x, int y)
        {
            X = x;
            Y = y;
        }
        // Calculate the absolute value of an integer
        public static int Abs(int x){return x > 0 ? x : -x;}
        // Calculate the distance between e1 and e2
        public static int Distance(Entity e1, Entity e2)
        {
            return Abs(e1.X - e2.X) + Abs(e1.Y - e2.Y);
        }
        // Returns true if the distance between e1 and e2 is lower than dist
        public static bool CloseTo(Entity e1, Entity e2, int distance)
        {
            return Distance(e1,e2) < distance;
        }

        // Move
        /* Tips: The class 'Game' defines a method ValidPosition
         * Tipsbis: You should use the random object
         */
        protected void Move(Game game)
        {
            if (!HaveValidPosition(game)) return;
            
            int random = game._random.Next(8);
            switch (random)
            {
                case 0:
                    if (game.ValidPosition(X + 1, Y))
                        SetXY(X+1,Y);
                    else
                        Move(game);
                    break;
                case 1:
                    if (game.ValidPosition(X - 1, Y-1))
                        SetXY(X-1,Y-1);
                    else
                        Move(game);
                    break;
                case 2:
                    if (game.ValidPosition(X - 1, Y))
                        SetXY(X-1,Y);
                    else
                        Move(game);
                    break;
                case 3:
                    if (game.ValidPosition(X , Y+1))
                        SetXY(X,Y+1);
                    else
                        Move(game);
                    break;
                case 4:
                    if (game.ValidPosition(X , Y-1))
                        SetXY(X,Y-1);
                    else
                        Move(game);
        
                    break;
                case 5:
                    if (game.ValidPosition(X +1, Y+1))
                        SetXY(X+1,Y+1);
                    else
                        Move(game);
            
                    break;
                case 6:
                    if (game.ValidPosition(X + 1, Y-1))
                        SetXY(X+1,Y-1);
                    else
                        Move(game);
                    break;
                case 7:
                    if (game.ValidPosition(X - 1, Y+1))
                        SetXY(X-1,Y+1);
                    else
                        Move(game);
                    break;
             
            }
      
        }

        private bool HaveValidPosition(Game game)
        {
            if (game.ValidPosition(X + 1, Y) || game.ValidPosition(X, Y + 1) || game.ValidPosition(X - 1, Y) ||
                game.ValidPosition(X, Y - 1))
                return true;
            if (game.ValidPosition(X + 1, Y+1) || game.ValidPosition(X-1, Y + 1) || game.ValidPosition(X + 1, Y-1) ||
                game.ValidPosition(X-1, Y - 1))
                return true;
            return false;
        }

        // MoveTo
        /* Tips: The class 'Game' defines a method ValidPosition
         */
        protected void MoveTo(Game game, Entity entity)
        {

            if (X == entity.X)
            {
                int newX = X;
                int newY = entity.Y < Y ? Y - 1 : Y + 1;
                if(game.ValidPosition(newX,newY))
                    SetXY(newX,newY);
            }
            if (Y == entity.Y)
            {
                int newY = Y;
                int newX = entity.X < X ? X - 1 : X + 1;
                if(game.ValidPosition(newX,newY))
                    SetXY(newX,newY);
            }

            if (entity.X > X && entity.Y > Y)
            {
                if (game.ValidPosition(X + 1, Y + 1))
                {
                    SetXY(X + 1, Y + 1);
                    return;
                }
                if (game.ValidPosition(X + 1, Y ))
                {
                    SetXY(X + 1, Y );
                    return;
                }
                if (game.ValidPosition(X , Y + 1))
                {
                    SetXY(X , Y + 1);
                    return;
                }
            }

            if (entity.X > X && entity.Y < Y)
            {
                if (game.ValidPosition(X + 1, Y - 1))
                {
                    SetXY(X + 1, Y - 1);
                    return;
                }
                if (game.ValidPosition(X + 1, Y ))
                {
                    SetXY(X + 1, Y );
                    return;
                }
                if (game.ValidPosition(X , Y - 1))
                {
                    SetXY(X , Y - 1);
                    return;
                }
            }

            if (entity.X < X && entity.Y > Y)
            {
                if (game.ValidPosition(X - 1, Y + 1))
                {
                    SetXY(X - 1, Y + 1);
                    return;
                }
                if (game.ValidPosition(X - 1, Y ))
                {
                    SetXY(X - 1, Y );
                    return;
                }
                if (game.ValidPosition(X , Y + 1))
                {
                    SetXY(X , Y + 1);
                    return;
                }
                
            }

            if (entity.X < X && entity.Y < Y)
            {
                if (game.ValidPosition(X - 1, Y - 1))
                {
                    SetXY(X - 1, Y - 1);
                    return;
                }
                if (game.ValidPosition(X - 1, Y ))
                {
                    SetXY(X - 1, Y );
                    return;
                }
                if (game.ValidPosition(X , Y - 1))
                {
                    SetXY(X, Y - 1);
                }
            }
            
           

        }
    }
}

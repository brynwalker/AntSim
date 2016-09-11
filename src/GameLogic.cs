﻿using SwinGameSDK;
using System;
using System.Collections.Generic;

namespace MyGame
{
    public static class GameLogic
    {
        private static List<Creature> _creatures = new List<Creature>();
        private static readonly Random _rand = new Random();
        private static List<IDrawable> _drawables = new List<IDrawable>();

        public static Random Random
        {
            get { return _rand; }
        }

        public static List<Creature> Creatures
        {
            get { return _creatures; }
            set { _creatures = value; }
        }

        public static void Process()
        {
            SwinGame.ProcessEvents();

            Point2D p = SwinGame.RandomScreenPoint();

            if (GameState.Setup)
            {
                for (int i = 0; i <= 1; i++)
                    _creatures.Add(new Creature(new Location(_rand.Next(720), _rand.Next(480))));

                foreach (Creature c in _creatures)
                {
                    c.NewPath = c.GetPathTo(new Location(_rand.Next(720), _rand.Next(480)));
                    c.CurrentPath = c.NewPath;
                    _drawables.Add(c);
                    _drawables.Add(c.CurrentPath.Waypoints.Last.Value);
                }

                GameState.Setup = false;
            }

            foreach (Creature c in _creatures)
                c.Move();
        }

        public static void DrawObjects()
        {
            foreach (IDrawable obj in _drawables)
                obj.Draw();
        }
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using System;

namespace StarterGame
{
    /*
     * Spring 2024
     */
    public class Game
    {
        private Player _player;
        private Parser _parser;
        private bool _playing;

        public Game()
        {
            GameWorld gm = new GameWorld();
            _playing = false;
            _parser = new Parser(new CommandWords());
            _player = new Player(gm.Entrance);
        }

        

        /**
     *  Main play routine.  Loops until end of play.
     */
        public void Play()
        {

            // Enter the main command loop.  Here we repeatedly read commands and
            // execute them until the game is over.
            if(_playing)
            {
                bool finished = false;
                while (!finished)
                {
                    Console.Write("\n>");
                    Command command = _parser.ParseCommand(Console.ReadLine());
                    if (command == null)
                    {
                        _player.ErrorMessage("I don't understand...");
                    }
                    else
                    {
                        finished = command.Execute(_player);
                    }
                }
            }

        }


        public void Start()
        {
            _playing = true;
            _player.InfoMessage(Welcome());
        }

        public void End()
        {
            _playing = false;
            _player.InfoMessage(Goodbye());
        }

        public string Welcome()
        {
            return "Welcome to the World of CSU!\n\n The World of CSU is a new, incredibly boring adventure game.\n\nType 'help' if you need help." + _player.CurrentRoom.Description();
        }

        public string Goodbye()
        {
            return "\nThank you for playing, Goodbye. \n";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
    public class Game
    {
        Player player;
        Parser parser;
        bool playing;
        GameClock gc;
        public Game()
        {
            //gc = new GameClock(5000);
            playing = false;
            parser = new Parser(new CommandWords());
            player = new Player(GameWorld.Instance.Entrance); //One world
        }



        /**
     *  Main play routine.  Loops until end of play.
     */
        public void Play()
        {

            // Enter the main command loop.  Here we repeatedly read commands and
            // execute them until the game is over.

            bool finished = false;
            while (!finished)
            {
                Console.Write("\n>");
                Command command = parser.ParseCommand(Console.ReadLine());
                if (command == null)
                {
                    Console.WriteLine("I don't understand...");
                }
                else
                {
                    finished = command.Execute(player);
                }
            }
        }

        public void GameOver()
        {
            if(player.Health <= 0)
            {
                playing = false;
                player.ErrorMessage("You died! Game Over.");
            }

        }
        public void Start()
        {
            playing = true;
            player.OutputMessage(Welcome());
        }

        public void End()
        {
            playing = false;
            player.OutputMessage(Goodbye());
        }
        public string Welcome()
        {
            return "Welcome to the Murder Mansion. You're an inexperienced Detective sent to investigate the murder of" +
                " Quincy Danielsson, the owner of the mansion. In order to win, you must figure out who " +
                "murdered Mr. Danielsson and kill the murderer before they kill you. If you kill 3 innocent people, you lose." +
                "\nThe mansion is also filled with booby traps that you'll have to find" +
                " your way around.\n" + player.CurrentRoom.Description();
        }

        public string Goodbye()
        {
            return "\nThank you for playing Murder Mansion, Goodbye. \n";
        }
    }
}

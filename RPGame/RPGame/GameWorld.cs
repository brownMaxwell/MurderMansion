using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
    //Singleton
    public class GameWorld
    {
        private static GameWorld _instance = null;

        public static GameWorld Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameWorld();
                }
                return _instance;
            }
        }

        private Room _entrance;
        public Room Entrance { get { return _entrance; } }
        private Room _exit;
        public Room Exit { get { return _exit; } }
        /*
        private Room _magicRoom;
        public Room MagicRoom { get { return _magicRoom; } }

        private Room _entertainmentRoom;

        private Room _toEntertainment;
        */
        //private Dictionary<Room, WorldEvent> worldEvents;
        //public Dictionary<string, IPeople> NPCs;


        private GameWorld()
        {

            CreateWorld();
            //worldEvents = new Dictionary<Room, WorldEvent>();
            //NPCs = new Dictionary<string, IPeople>();
           // NotificationCenter.Instance.AddObserver("PlayerWillExitRoom", PlayerWillExitRoom);
            //NotificationCenter.Instance.AddObserver("PlayerDidExitRoom", PlayerDidExitRoom);
            //NotificationCenter.Instance.AddObserver("GameClockTick", GameClockTick);
        }
        /*
        public void PlayerWillExitRoom(Notification notification)
        {
            Player player = (Player)notification.Object;
            player.OutputMessage("PLayer will exit " + player.CurrentRoom.Tag);
        }
        
        /*
        public void PlayerDidExitRoom(Notification notification)
        {
            Player player = (Player)notification.Object;
            WorldEvent we = null;
            worldEvents.TryGetValue(player.CurrentRoom, out we);
            if (player.CurrentRoom == _exit)
            {
                we.Execute();
            }
            

        }
        /*
        public void GameClockTick(Notification notification)
        {
            Console.WriteLine("Tick!");
        }
        */
        private void CreateWorld()
        {
            
            Room Entrance = new Room("Entry way");
            Room Atrium = new Room("Atrium");           
            Door door = Door.connectRooms(Entrance, Atrium, "south", "north");

            Room MainHallFirstFloor = new Room("Main Hall");
            door = Door.connectRooms(Atrium, MainHallFirstFloor, "south", "north");
            Room Stairwell = new Room("Stairwell");
            door = Door.connectRooms(Stairwell, MainHallFirstFloor, "north", "south");

            Room FirstFloorStudy = new Room("First Floor Study");
            door = Door.connectRooms(FirstFloorStudy, MainHallFirstFloor, "east", "west");           
            ILocking studyLock = new RegularLock();           
            studyLock.Lock();
            door.InstallLock(studyLock);
            door.Close();
            door.Lock();

            Room GuestBedroom = new Room("Guest Bedroom");
            door = Door.connectRooms(GuestBedroom, MainHallFirstFloor, "west", "east");         
            ILocking guestLock = new RegularLock();
            guestLock.Lock();
            door.InstallLock(guestLock);
            door.Close();
            door.Lock();

            Room GuestBathroom = new Room("Guest Bathroom");
            door = Door.connectRooms(GuestBedroom, GuestBathroom, "east", "west");

            Room SwimmingPool = new Room("Swimming Pool");
            door = Door.connectRooms(SwimmingPool, Atrium, "west", "east");
            
            Room Sauna = new Room("Sauna");
            door = Door.connectRooms(Sauna, SwimmingPool, "south", "north");
            Room HotTub = new Room("Hot Tub");
            door = Door.connectRooms(HotTub, Sauna, "west", "east");
            Room Gym = new Room("Gym");
            door = Door.connectRooms(Gym, HotTub, "west", "east");

            Room MainHallSecondFloor = new Room("Main Hall Second Floor");
            door = Door.connectRooms(Stairwell, MainHallSecondFloor, "south", "north");
            Room ServantQuartersOne = new Room("Servant's Quarters");

            door = Door.connectRooms(ServantQuartersOne, Atrium, "east", "west");           
            ILocking servantLock = new RegularLock();
            servantLock.Lock();
            door.InstallLock(servantLock);
            door.Close();
            door.Lock();

            Room ServantBathroom = new Room("Servants' Quarters Bathroom");
            door = Door.connectRooms(ServantBathroom, ServantQuartersOne, "east", "west");
            Room ServantQuartersTwo = new Room("Servants' Quarters Continued");
            door = Door.connectRooms(ServantQuartersTwo, ServantBathroom, "north", "south");

            Room Lounge = new Room("Lounge");
            door = Door.connectRooms(Lounge, SwimmingPool, "west", "east");

            Room SecondFloorStudy = new Room("Second Floor Study");
            door = Door.connectRooms(MainHallSecondFloor, SecondFloorStudy, "south", "north");

            Room SecondFloorGuest = new Room("Second Floor Guest Bedroom");
            door = Door.connectRooms(MainHallSecondFloor, SecondFloorGuest, "east", "west");

            Room KitchenSecondFloor = new Room("Main Kitchen");
            door = Door.connectRooms(MainHallSecondFloor, KitchenSecondFloor, "west", "east");

            Room LoungeSecond = new Room("Second Floor Lounge");
            door = Door.connectRooms(KitchenSecondFloor, LoungeSecond, "south", "north");
            RoomDelegate teleport = new TrapDoorRoom();
            teleport.ContainingRoom = LoungeSecond;
            teleport.PossibleRooms.Add(MainHallFirstFloor);
            teleport.PossibleRooms.Add(Gym);
            teleport.PossibleRooms.Add(Lounge);
            teleport.PossibleRooms.Add(HotTub);
            teleport.PossibleRooms.Add(Atrium);
            teleport.PossibleRooms.Add(Entrance);
            teleport.PossibleRooms.Add(Sauna);
            LoungeSecond.Delegate = teleport;

            Room ServantQuartersOneFloorTwo = new Room("Servant's Quarters Second Floor");
            door = Door.connectRooms(LoungeSecond, ServantQuartersOneFloorTwo, "west", "east");
            Room ServantBathroomSecondFloor = new Room("Servants' Quarters Bathroom");
            door = Door.connectRooms(ServantQuartersOneFloorTwo, ServantBathroomSecondFloor, "north", "south");
            Room ServantQuartersTwoSecondFloor = new Room("Servants' Quarters Second Floor Continued");
            door = Door.connectRooms(ServantQuartersTwoSecondFloor, ServantBathroomSecondFloor, "east", "west");
            TrapRoom secondStudyTrap = new TrapRoom();
            secondStudyTrap.Password = "Stacy";
            secondStudyTrap.ContainingRoom = SecondFloorStudy;
            SecondFloorStudy.Delegate = secondStudyTrap;

            
            /*
             * Items
             */
            IItem playerGun = new Revolver("Revolver", 2.0f, 1.0f, true);
            //IItem clone = playerGun; //Convenient Revolver for testing.
           // Atrium.HasItem(clone);
            IItem murderGun = new Pistol("Pistol", 1.0f, 1.0f, false); //Equipped to murderer;
            FirstFloorStudy.HasItem(playerGun);
            IItem StudyKey = new Key("Study_Key", 0.2f, 0.1f, true, FirstFloorStudy); //James.
            IItem GuestKey = new Key("Guest_Key", 0.2f, 0.1f, true, GuestBedroom); //In Wardrobe in Lounge.
            IItem ServantKey = new Key("Servant_Quarters_Key", 0.2f, 0.1f, true, ServantQuartersOne); //In Dresser in SecondFloorGuest
            IItem Jimbo = new Item("Jimbo", 0.5f, 2.5f, true); //In SecondFloorStudy
            IItem SwimSuit = new Item("Swimsuit", 0.1f, 2.5f, true); //In Guest Bathroom
            IItem Scope = new Item("Scope", 1.0f, 1.0f, true); //wardrobe Lounge / Adds 2 accuracy to Modified weapon.
            IItem LightFrame = new Item("Light_Frame", -1.0f, 0.0f, true); //Stacy / Reduces weight of Modified weapon by 1.
            IItem Wardrobe = new ItemContainer("Wooden_Wardrobe", 100.0f, 100.0f, false);
            Wardrobe.AddItem(Scope);
            Wardrobe.AddItem(GuestKey);
            Lounge.HasItem(Wardrobe);
            IItem Dresser = new ItemContainer("Dresser", 150.0f, 100.0f, false);
            Dresser.AddItem(ServantKey);
            SecondFloorGuest.HasItem(Dresser);
            
            /*
             * NPC's
             */
            NPC James = new NPC("James Conroy", Atrium, true);
            James.DialogueOptions.Add("Greeting", "\nWell 'ello there Mr. Detective. I'm " +
                "mighty glad you're here to figure out who murdered Mr. Danielsson.");
            James.DialogueOptions.Add("Accuse", "\nI'd nevah, I tell ya! I loved Mr. Danielsson like a brother!");
            James.DialogueOptions.Add("Relationship", "\nHe was my best friend! I'm bloody tore up about it I tell ya.");
            James.DialogueOptions.Add("Suspicion", "\nIf I had to point a finger, I'd have to say it was probably Stacy." +
                " She and Quincy had somethin a lil spicy goin on, but he wanted to end things. She didn't seem too happy about it.");
            James.DialogueOptions.Add("Insult", "\nWell why'd you have to go n do that? I'm just a modest man tryna grieve" +
                " me friend 'ere.");
            James.DialogueOptions.Add("Greeting Advanced", "\nDetective! I hope you've made some good progress, yeah?");
            James.DialogueOptions.Add("Accuse Advanced", "\nC'mon Detective! I'd think you'd know I'm innocent by now!");
            James.DialogueOptions.Add("Relationship Advanced", "\nQuincy was always too nice to me. I was a bit of a leech.");
            James.DialogueOptions.Add("Suspicion Advanced", "\nMy noggin says it's Stacy, but somethin' in me gut says it's Guinevere, " +
                "his mum.");
            James.DialogueOptions.Add("Insult Advanced", "\nAnd 'ere I thought we were becoming fast friends.");           
            /*
             * James' Quest
             */
            Quest StudyKeyJames = new Quest("Mr. Detective, if you could do me the favor of finding my old " +
                "teddy bear Jimbo, I'd be more than happy to give you the key to the First Floor Study room.", James, "Jimbo", "Study_Key");
            SecondFloorStudy.HasItem(Jimbo);
            James.Task = StudyKeyJames;
            James.DialogueOptions.Add("Quest", (James.Task == null ? "" : James.Task.Description));
            James.DialogueOptions.Add("Quest Advanced", (James.Task == null ? "" : James.Task.Description));
            James.GetItem(StudyKey);
            Atrium.SetNPC(James);
            
            NPC Guinevere = new NPC("Guinevere Danielsson", GuestBedroom, false);
            Guinevere.DialogueOptions.Add("Greeting", "\nHello detective. I'm Guinevere Danielsson, mother of Quincy.");
            Guinevere.DialogueOptions.Add("Accuse", "\nHow dare you! To think I'd hurt my own baby boy... Blasphemy!");
            Guinevere.DialogueOptions.Add("Relationship", "\nQuincy is...was my dearest son. He and I had always been very close since he was born.");
            Guinevere.DialogueOptions.Add("Suspicion", "\nThat no brainer James was always jealous of my Quincy. I wouldn't be shocked to find out " +
                "that freeloader had something to do with my son's untimely demise.");
            Guinevere.DialogueOptions.Add("Insult", "\nI will take no such insult from a backwater Detective such as yourself.");
            Guinevere.DialogueOptions.Add("Greeting Advanced", "\nHello Detective. Have you made any progress on my son's case? Please do keep me in the loop.");
            Guinevere.DialogueOptions.Add("Accuse Advanced", "\nIf you insist on accusing me any further, I will have you forcibly removed from the property!");
            Guinevere.DialogueOptions.Add("Relationship Advanced", "\nHe was always such a bright young boy. If only he hadn't squandered his potential" +
                " by sullying himself with terrible company.");
            Guinevere.DialogueOptions.Add("Suspicion Advanced", "\nIt had to be that wench Stacy!");
            Guinevere.DialogueOptions.Add("Insult Advanced", "\nYou'd do well to watch your tongue.");
            Guinevere.EquipGun(murderGun);
            GuestBedroom.SetNPC(Guinevere);

            NPC Benjamin = new NPC("Benjamin Fingerlakes", ServantQuartersOne, true);
            Benjamin.DialogueOptions.Add("Greeting", "\nG'day, Detective. I'm Benjamin Fingerlakes, the head butler of this estate.");
            Benjamin.DialogueOptions.Add("Accuse", "\nWould not be wise of me to murder my boss.");
            Benjamin.DialogueOptions.Add("Relationship", "\nMr. Danielsson was always good to me. I practially raised that boy.");
            Benjamin.DialogueOptions.Add("Suspicion", "\nI honestly haven't the faintest idea. Everyone here loved Mr. Danielsson, in their own way.");
            Benjamin.DialogueOptions.Add("Insult", "\nI'm a professional so I'll pretend I didn't hear that.");
            Benjamin.DialogueOptions.Add("Greeting Advanced", "\nAh, Detective. How can I help you?");
            Benjamin.DialogueOptions.Add("Accuse Advanced", "\nI'm starting to doubt your detective skills, if I'm to be honest with you.");
            Benjamin.DialogueOptions.Add("Relationship Advanced", "\nThere were certainly... bad days to be serving Mr. Danielsson, but nothing one would kill over.");
            Benjamin.DialogueOptions.Add("Suspicion Advanced", "\nI honestly have no idea.");
            Benjamin.DialogueOptions.Add("Insult Advanced", "\nAnd with that, I do ask that you refrain from speaking such falsehoods around my workplace.");
            ServantQuartersOne.SetNPC(Benjamin);

            NPC Stacy = new NPC("Stacy Mathers", SwimmingPool, true);
            Stacy.DialogueOptions.Add("Greeting", "\nHowdy detective! I'm glad someone is here to put a murderer behind bars.");
            Stacy.DialogueOptions.Add("Accuse", "\nGood heavens, no! I'd never hurt a fly!");
            Stacy.DialogueOptions.Add("Relationship", "\nWe were...an 'item', but we stopped seein' eachother a while back." +
                " Some people think I'm a prime suspect because of this, but I was relieved when Quincy said he wanted to end things." +
                " He was so gosh darn needy.");
            Stacy.DialogueOptions.Add("Suspicion", "\nThis may sound strange, but something about Quincy's mother never seemed right to me." +
                " No woman was ever good enough for her little boy.");
            Stacy.DialogueOptions.Add("Insult", "\nWell aren't you a crude Detective.");
            Stacy.DialogueOptions.Add("Greeting Advanced", "\nHowdy detective! You any closer to solvin this case?");
            Stacy.DialogueOptions.Add("Accuse Advanced", "\nOh shucks detective. I'm just tryna live my life I aint no killer.");
            Stacy.DialogueOptions.Add("Relationship Advanced", "\nI tell ya though, I was real tired of Quincy always tryna act like I needed him. What nonsense.");
            Stacy.DialogueOptions.Add("Suspicion Advanced", "\nI'm tellin ya it's Guinevere. I wouldn't be surprised if it was ol' Benjamin though.");
            Stacy.DialogueOptions.Add("Insult Advanced", "\nYou wanna say that again? See what happens cowboy.");
            /*
             * Stacy Quest
             */
            Quest StacyQuest = new Quest("\nI went n forgot my Swimsuit in the Guest Bathroom! Be a doll and fetch it for me would ya? " +
                "I'll give you this Modification for your gun.", Stacy, "Swimsuit", "Light_Frame");
            GuestBathroom.HasItem(SwimSuit);
            Stacy.Task = StacyQuest;
            Stacy.DialogueOptions.Add("Quest", (Stacy.Task == null ? "" : Stacy.Task.Description));
            Stacy.DialogueOptions.Add("Quest Advanced", (Stacy.Task == null ? "" : Stacy.Task.Description));
            Stacy.GetItem(LightFrame);
            SwimmingPool.SetNPC(Stacy);

            NPC Helga = new NPC("Helga Budweiser", SecondFloorGuest, true);
            Helga.DialogueOptions.Add("Greeting", "\nOh ya very good evening we are having Mr. Detective.");
            Helga.DialogueOptions.Add("Accuse", "\nlNein! I'm innocent!");
            Helga.DialogueOptions.Add("Relationship", "\nQuincy's mother wanted me to marry him. I do not know him very well.");
            Helga.DialogueOptions.Add("Suspicion", "\nI do not know anyone here besides the mother. She is very nice to me though.");
            Helga.DialogueOptions.Add("Insult", "\nArsch Mit Ohren!");
            Helga.DialogueOptions.Add("Greeting Advanced", "\nGutentag, Detective!");
            Helga.DialogueOptions.Add("Accuse Advanced", "\nNein! I was not present when the man was killed, I swear.");
            Helga.DialogueOptions.Add("Relationship Advanced", "\nI did not want to marry Quincy, but I did not kill him.");
            Helga.DialogueOptions.Add("Suspicion Advanced", "\nZe butler man is weird.");
            Helga.DialogueOptions.Add("Insult Advanced", "\nKotzbrocken!");
            SecondFloorGuest.SetNPC(Helga);

            NPC George = new NPC("George Townsman", MainHallSecondFloor, true);
            George.DialogueOptions.Add("Greeting", "\nNice to meet you, Detective. I'm George Townsman.");
            George.DialogueOptions.Add("Accuse", "\nThat's fair. We're all suspects here. All I can say is that I didn't do it.");
            George.DialogueOptions.Add("Relationship", "\nI'm a friend of his father. I've been staying with the Danielsson's for a bit.");
            George.DialogueOptions.Add("Suspicion", "\nI can't imagine any of the residents doing this. Perhaps the help should be looked into.");
            George.DialogueOptions.Add("Insult", "\nWell right back at you, detective.");
            George.DialogueOptions.Add("Greeting Advanced", "\nHow goes the investigation? I hope it's been fuitful.");
            George.DialogueOptions.Add("Accuse Advanced", "\nI wouldn't kill anyone, let alone the son of my friend.");
            George.DialogueOptions.Add("Relationship Advanced", "\nHe was like a nephew to me.");
            George.DialogueOptions.Add("Suspicion Advanced", "\nPerhaps the German girl, Helga, did not want to be married to him.");
            George.DialogueOptions.Add("Insult Advanced", "\nSurely you're joking.");
            MainHallSecondFloor.SetNPC(George);


            _entrance = Entrance;
            //Instance.NPCs.Add("Stacy", Stacy);
            //Instance.NPCs.Add("James", James);
        }
        
     

        private class WorldChange : WorldEvent
        {
            public Room roomA { get; set; }
            public Room roomB { get; set; }
            public string aTob { get; set; }
            public string bToa { get; set; }

            public WorldChange(Room roomA, Room roomB, string aTob, string bToa)
            {
                roomA = roomA;
                roomB = roomB;
                aTob = aTob;
                bToa = bToa;
            }

            public void Execute()
            {
                Door.connectRooms(roomA, roomB, aTob, bToa);
            }

        }
    }
}

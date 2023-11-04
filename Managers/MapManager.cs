using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Roguelite_Game.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelite_Game.Managers
{
    public class MapManager
    {
        private int _rooms;
        private int _floor;
        private int _itemRooms = 2;
        private int _bossRooms = 1;
        private Random rnd = new Random();

        private int _branchRoomChance = 6;

        public int Rooms { get { return _rooms; } }
        public Bullet Bullet { get; set; }
        public List<Room> RoomsList;

        public MapManager(ContentManager content, int floor, int rooms)
        {
            _floor = floor;
            _rooms = rooms;
            RoomsList = new List<Room>();

            RoomsList.Add(new Room(content, new Vector2(0, 0)));
            RoomsList[0].isVisible = true;
            int roomCount = 1;
            int?[,] leftRoomsIndex = new int?[_rooms, 4];
            int?[,] rightRoomsIndex = new int?[_rooms, 4];
            int?[,] upRoomsIndex = new int?[_rooms, 4];
            int?[,] downRoomsIndex = new int?[_rooms, 4];
            while (roomCount < _rooms)
            {
                var direction = rnd.Next(1, 5); //1 = Left. 2 = Up. 3 = Right. 4 = Down
                                                //0 = Left. 1 = Up. 2 = Right. 3 = Down for Array Indexing

                //Enemy Room Generation
                if (roomCount < _rooms - _itemRooms - _bossRooms)
                {
                    if (direction == 1) //LEFT
                    {
                        for (int i = 0; i < _rooms; i++)
                        {
                            if (i == 0 && leftRoomsIndex[0, 0] == null)
                            {
                                Vector2 offset = new Vector2(-1, 0);
                                var enemyRoom = new EnemyRoom(content, offset, new EnemyManager(content, offset) { Bullet = Bullet, });
                                RoomsList.Add(enemyRoom);
                                leftRoomsIndex[0, 0] = RoomsList.Count - 1;
                                RoomsList[(int)leftRoomsIndex[0, 0]].GenerateDoor("right", RoomsList[0]);
                                RoomsList[0].GenerateDoor("left", RoomsList[(int)leftRoomsIndex[0, 0]]);
                                roomCount++;
                                break;
                            }
                            else if (leftRoomsIndex[i, 0] != null)
                            {
                                if (rnd.Next(1, 11) <= _branchRoomChance)
                                {
                                    var upOrDown = rnd.Next(1, 3);
                                    if (upOrDown == 1 && leftRoomsIndex[i, 3] == null) //Checks if a room is below
                                    {
                                        Vector2 offset = new Vector2(-(i + 1), 1);
                                        var enemyRoom = new EnemyRoom(content, offset, new EnemyManager(content, offset) { Bullet = Bullet, });
                                        if (i == 0)
                                        {
                                            if (downRoomsIndex[i, 0] != null)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                RoomsList.Add(enemyRoom);
                                                leftRoomsIndex[i, 3] = RoomsList.Count - 1;
                                                RoomsList[(int)leftRoomsIndex[i, 3]].GenerateDoor("up", RoomsList[(int)leftRoomsIndex[i, 0]]);
                                                RoomsList[(int)leftRoomsIndex[i, 0]].GenerateDoor("down", RoomsList[(int)leftRoomsIndex[i, 3]]);
                                                roomCount++;
                                                break;
                                            }
                                        }
                                        RoomsList.Add(enemyRoom);
                                        leftRoomsIndex[i, 3] = RoomsList.Count - 1;
                                        RoomsList[(int)leftRoomsIndex[i, 3]].GenerateDoor("up", RoomsList[(int)leftRoomsIndex[i, 0]]);
                                        RoomsList[(int)leftRoomsIndex[i, 0]].GenerateDoor("down", RoomsList[(int)leftRoomsIndex[i, 3]]);
                                        roomCount++;
                                        break;
                                    }
                                    else if (upOrDown == 2 && leftRoomsIndex[i, 1] == null) //Checks if a room is above
                                    {
                                        Vector2 offset = new Vector2(-(i + 1), -1);
                                        var enemyRoom = new EnemyRoom(content, offset, new EnemyManager(content, offset) { Bullet = Bullet, });
                                        if (i == 0)
                                        {
                                            if (upRoomsIndex[i, 0] != null)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                RoomsList.Add(enemyRoom);
                                                leftRoomsIndex[i, 1] = RoomsList.Count - 1;
                                                RoomsList[(int)leftRoomsIndex[i, 1]].GenerateDoor("down", RoomsList[(int)leftRoomsIndex[i, 0]]);
                                                RoomsList[(int)leftRoomsIndex[i, 0]].GenerateDoor("up", RoomsList[(int)leftRoomsIndex[i, 1]]);
                                                roomCount++;
                                                break;
                                            }
                                        }
                                        RoomsList.Add(enemyRoom);
                                        leftRoomsIndex[i, 1] = RoomsList.Count - 1;
                                        RoomsList[(int)leftRoomsIndex[i, 1]].GenerateDoor("down", RoomsList[(int)leftRoomsIndex[i, 0]]);
                                        RoomsList[(int)leftRoomsIndex[i, 0]].GenerateDoor("up", RoomsList[(int)leftRoomsIndex[i, 1]]);
                                        roomCount++;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                Vector2 offset = new Vector2(-(i + 1), 0);
                                var enemyRoom = new EnemyRoom(content, offset, new EnemyManager(content, offset) { Bullet = Bullet, });
                                RoomsList.Add(enemyRoom);
                                leftRoomsIndex[i, 0] = RoomsList.Count - 1;
                                RoomsList[(int)leftRoomsIndex[i, 0]].GenerateDoor("right", RoomsList[(int)leftRoomsIndex[i - 1, 0]]);
                                RoomsList[(int)leftRoomsIndex[i - 1, 0]].GenerateDoor("left", RoomsList[(int)leftRoomsIndex[i, 0]]);
                                roomCount++;
                                break;
                            }
                        }

                    }

                    else if (direction == 2) // UP
                    {
                        for (int i = 0; i < _rooms; i++)
                        {
                            if (i == 0 && upRoomsIndex[0, 1] == null)
                            {
                                Vector2 offset = new Vector2(0, -1);
                                var enemyRoom = new EnemyRoom(content, offset, new EnemyManager(content, offset) { Bullet = Bullet, });
                                RoomsList.Add(enemyRoom);
                                upRoomsIndex[0, 1] = RoomsList.Count - 1;
                                RoomsList[(int)upRoomsIndex[0, 1]].GenerateDoor("down", RoomsList[0]);
                                RoomsList[0].GenerateDoor("up", RoomsList[(int)upRoomsIndex[0, 1]]);
                                roomCount++;
                                break;
                            }
                            else if (upRoomsIndex[i, 1] != null)
                            {
                                if (i < upRoomsIndex.Length - 1)
                                {
                                    if (rnd.Next(1, 11) <= _branchRoomChance)
                                    {
                                        var leftOrRight = rnd.Next(1, 3);
                                        if (leftOrRight == 1 && upRoomsIndex[i, 0] == null) //Checks if a room is to the left
                                        {
                                            Vector2 offset = new Vector2(-1, -(i + 1));
                                            var enemyRoom = new EnemyRoom(content, offset, new EnemyManager(content, offset) { Bullet = Bullet, });
                                            if (i == 0)
                                            {
                                                if (leftRoomsIndex[i, 1] != null)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    RoomsList.Add(enemyRoom);
                                                    upRoomsIndex[i, 0] = RoomsList.Count - 1;
                                                    RoomsList[(int)upRoomsIndex[i, 0]].GenerateDoor("right", RoomsList[(int)upRoomsIndex[i, 1]]);
                                                    RoomsList[(int)upRoomsIndex[i, 1]].GenerateDoor("left", RoomsList[(int)upRoomsIndex[i, 0]]);
                                                    roomCount++;
                                                    break;
                                                }
                                            }
                                            RoomsList.Add(enemyRoom);
                                            upRoomsIndex[i, 0] = RoomsList.Count - 1;
                                            RoomsList[(int)upRoomsIndex[i, 0]].GenerateDoor("right", RoomsList[(int)upRoomsIndex[i, 1]]);
                                            RoomsList[(int)upRoomsIndex[i, 1]].GenerateDoor("left", RoomsList[(int)upRoomsIndex[i, 0]]);
                                            roomCount++;
                                            break;
                                        }
                                        else if (leftOrRight == 2 && upRoomsIndex[i, 2] == null) //Checks if a room is to the right
                                        {
                                            Vector2 offset = new Vector2(1, -(i + 1));
                                            var enemyRoom = new EnemyRoom(content, offset, new EnemyManager(content, offset) { Bullet = Bullet, });
                                            if (i == 0)
                                            {
                                                if (rightRoomsIndex[i, 1] != null)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    RoomsList.Add(enemyRoom);
                                                    upRoomsIndex[i, 2] = RoomsList.Count - 1;
                                                    RoomsList[(int)upRoomsIndex[i, 2]].GenerateDoor("left", RoomsList[(int)upRoomsIndex[i, 1]]);
                                                    RoomsList[(int)upRoomsIndex[i, 1]].GenerateDoor("right", RoomsList[(int)upRoomsIndex[i, 2]]);
                                                    roomCount++;
                                                    break;
                                                }
                                            }
                                            RoomsList.Add(enemyRoom);
                                            upRoomsIndex[i, 2] = RoomsList.Count - 1;
                                            RoomsList[(int)upRoomsIndex[i, 2]].GenerateDoor("left", RoomsList[(int)upRoomsIndex[i, 1]]);
                                            RoomsList[(int)upRoomsIndex[i, 1]].GenerateDoor("right", RoomsList[(int)upRoomsIndex[i, 2]]);
                                            roomCount++;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Vector2 offset = new Vector2(0, -(i + 1));
                                var enemyRoom = new EnemyRoom(content, offset, new EnemyManager(content, offset) { Bullet = Bullet, });
                                RoomsList.Add(enemyRoom);
                                upRoomsIndex[i, 1] = RoomsList.Count - 1;
                                RoomsList[(int)upRoomsIndex[i, 1]].GenerateDoor("down", RoomsList[(int)upRoomsIndex[i - 1, 1]]);
                                RoomsList[(int)upRoomsIndex[i - 1, 1]].GenerateDoor("up", RoomsList[(int)upRoomsIndex[i, 1]]);
                                roomCount++;
                                break;
                            }
                        }

                    }

                    else if (direction == 3) //RIGHT
                    {
                        for (int i = 0; i < _rooms; i++)
                        {
                            if (i == 0 && rightRoomsIndex[0, 2] == null)
                            {
                                Vector2 offset = new Vector2(1, 0);
                                var enemyRoom = new EnemyRoom(content, offset, new EnemyManager(content, offset) { Bullet = Bullet, });
                                RoomsList.Add(enemyRoom);
                                rightRoomsIndex[0, 2] = RoomsList.Count - 1;
                                RoomsList[(int)rightRoomsIndex[0, 2]].GenerateDoor("left", RoomsList[0]);
                                RoomsList[0].GenerateDoor("right", RoomsList[(int)rightRoomsIndex[0, 2]]);
                                roomCount++;
                                break;
                            }
                            else if (rightRoomsIndex[i, 2] != null)
                            {
                                if (i < rightRoomsIndex.Length - 1)
                                {
                                    if (rnd.Next(1, 11) <= _branchRoomChance)
                                    {
                                        var upOrDown = rnd.Next(1, 3);
                                        if (upOrDown == 1 && rightRoomsIndex[i, 3] == null) //Checks if a room is below
                                        {
                                            Vector2 offset = new Vector2((i + 1), 1);
                                            var enemyRoom = new EnemyRoom(content, offset, new EnemyManager(content, offset) { Bullet = Bullet, });
                                            if (i == 0)
                                            {
                                                if (downRoomsIndex[i, 2] != null)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    RoomsList.Add(enemyRoom);
                                                    rightRoomsIndex[i, 3] = RoomsList.Count - 1;
                                                    RoomsList[(int)rightRoomsIndex[i, 3]].GenerateDoor("up", RoomsList[(int)rightRoomsIndex[i, 2]]);
                                                    RoomsList[(int)rightRoomsIndex[i, 2]].GenerateDoor("down", RoomsList[(int)rightRoomsIndex[i, 3]]);
                                                    roomCount++;
                                                    break;
                                                }
                                            }
                                            RoomsList.Add(enemyRoom);
                                            rightRoomsIndex[i, 3] = RoomsList.Count - 1;
                                            RoomsList[(int)rightRoomsIndex[i, 3]].GenerateDoor("up", RoomsList[(int)rightRoomsIndex[i, 2]]);
                                            RoomsList[(int)rightRoomsIndex[i, 2]].GenerateDoor("down", RoomsList[(int)rightRoomsIndex[i, 3]]);
                                            roomCount++;
                                            break;
                                        }
                                        else if (upOrDown == 2 && rightRoomsIndex[i, 1] == null) //Checks if a room is above
                                        {
                                            Vector2 offset = new Vector2((i + 1), -1);
                                            var enemyRoom = new EnemyRoom(content, offset, new EnemyManager(content, offset) { Bullet = Bullet, });
                                            if (i == 0)
                                            {
                                                if (upRoomsIndex[i, 2] != null)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    RoomsList.Add(enemyRoom);
                                                    rightRoomsIndex[i, 1] = RoomsList.Count - 1;
                                                    RoomsList[(int)rightRoomsIndex[i, 1]].GenerateDoor("down", RoomsList[(int)rightRoomsIndex[i, 2]]);
                                                    RoomsList[(int)rightRoomsIndex[i, 2]].GenerateDoor("up", RoomsList[(int)rightRoomsIndex[i, 1]]);
                                                    roomCount++;
                                                    break;
                                                }
                                            }
                                            RoomsList.Add(enemyRoom);
                                            rightRoomsIndex[i, 1] = RoomsList.Count - 1;
                                            RoomsList[(int)rightRoomsIndex[i, 1]].GenerateDoor("down", RoomsList[(int)rightRoomsIndex[i, 2]]);
                                            RoomsList[(int)rightRoomsIndex[i, 2]].GenerateDoor("up", RoomsList[(int)rightRoomsIndex[i, 1]]);
                                            roomCount++;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Vector2 offset = new Vector2((i + 1), 0);
                                var enemyRoom = new EnemyRoom(content, offset, new EnemyManager(content, offset) { Bullet = Bullet, });
                                RoomsList.Add(enemyRoom);
                                rightRoomsIndex[i, 2] = RoomsList.Count - 1;
                                RoomsList[(int)rightRoomsIndex[i, 2]].GenerateDoor("left", RoomsList[(int)rightRoomsIndex[i - 1, 2]]);
                                RoomsList[(int)rightRoomsIndex[i - 1, 2]].GenerateDoor("right", RoomsList[(int)rightRoomsIndex[i, 2]]);
                                roomCount++;
                                break;
                            }
                        }
                    }

                    else if (direction == 4) //DOWN
                    {
                        for (int i = 0; i < _rooms; i++)
                        {
                            if (i == 0 && downRoomsIndex[0, 3] == null)
                            {
                                Vector2 offset = new Vector2(0, 1);
                                var enemyRoom = new EnemyRoom(content, offset, new EnemyManager(content, offset) { Bullet = Bullet, });
                                RoomsList.Add(enemyRoom);
                                downRoomsIndex[0, 3] = RoomsList.Count - 1;
                                RoomsList[(int)downRoomsIndex[0, 3]].GenerateDoor("up", RoomsList[0]);
                                RoomsList[0].GenerateDoor("down", RoomsList[(int)downRoomsIndex[0, 3]]);
                                roomCount++;
                                break;
                            }
                            else if (downRoomsIndex[i, 3] != null)
                            {
                                if (i < downRoomsIndex.Length - 1)
                                {
                                    if (rnd.Next(1, 11) <= _branchRoomChance)
                                    {
                                        var leftOrRight = rnd.Next(1, 3);
                                        if (leftOrRight == 1 && downRoomsIndex[i, 0] == null) //Checks if a room is to the left
                                        {
                                            Vector2 offset = new Vector2(-1, (i + 1));
                                            var enemyRoom = new EnemyRoom(content, offset, new EnemyManager(content, offset) { Bullet = Bullet, });
                                            if (i == 0)
                                            {
                                                if (leftRoomsIndex[i, 3] != null)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    RoomsList.Add(enemyRoom);
                                                    downRoomsIndex[i, 0] = RoomsList.Count - 1;
                                                    RoomsList[(int)downRoomsIndex[i, 0]].GenerateDoor("right", RoomsList[(int)downRoomsIndex[i, 3]]);
                                                    RoomsList[(int)downRoomsIndex[i, 3]].GenerateDoor("left", RoomsList[(int)downRoomsIndex[i, 0]]);
                                                    roomCount++;
                                                    break;
                                                }
                                            }
                                            RoomsList.Add(enemyRoom);
                                            downRoomsIndex[i, 0] = RoomsList.Count - 1;
                                            RoomsList[(int)downRoomsIndex[i, 0]].GenerateDoor("right", RoomsList[(int)downRoomsIndex[i, 3]]);
                                            RoomsList[(int)downRoomsIndex[i, 3]].GenerateDoor("left", RoomsList[(int)downRoomsIndex[i, 0]]);
                                            roomCount++;
                                            break;
                                        }
                                        else if (leftOrRight == 2 && downRoomsIndex[i, 2] == null) //Checks if a room is to the right
                                        {
                                            Vector2 offset = new Vector2(1, (i + 1));
                                            var enemyRoom = new EnemyRoom(content, offset, new EnemyManager(content, offset) { Bullet = Bullet, });
                                            if (i == 0)
                                            {
                                                if (rightRoomsIndex[i, 3] != null)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    RoomsList.Add(enemyRoom);
                                                    downRoomsIndex[i, 2] = RoomsList.Count - 1;
                                                    RoomsList[(int)downRoomsIndex[i, 2]].GenerateDoor("left", RoomsList[(int)downRoomsIndex[i, 3]]);
                                                    RoomsList[(int)downRoomsIndex[i, 3]].GenerateDoor("right", RoomsList[(int)downRoomsIndex[i, 2]]);
                                                    roomCount++;
                                                    break;
                                                }
                                            }
                                            RoomsList.Add(enemyRoom);
                                            downRoomsIndex[i, 2] = RoomsList.Count - 1;
                                            RoomsList[(int)downRoomsIndex[i, 2]].GenerateDoor("left", RoomsList[(int)downRoomsIndex[i, 3]]);
                                            RoomsList[(int)downRoomsIndex[i, 3]].GenerateDoor("right", RoomsList[(int)downRoomsIndex[i, 2]]);
                                            roomCount++;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Vector2 offset = new Vector2(0, (i + 1));
                                var enemyRoom = new EnemyRoom(content, offset, new EnemyManager(content, offset) { Bullet = Bullet, });
                                RoomsList.Add(enemyRoom);
                                downRoomsIndex[i, 3] = RoomsList.Count - 1;
                                RoomsList[(int)downRoomsIndex[i, 3]].GenerateDoor("up", RoomsList[(int)downRoomsIndex[i - 1, 3]]);
                                RoomsList[(int)downRoomsIndex[i - 1, 3]].GenerateDoor("down", RoomsList[(int)downRoomsIndex[i, 3]]);
                                roomCount++;
                                break;
                            }
                        }
                    }
                }

                //Item Room Generation
                else if (roomCount < _rooms - _bossRooms)
                {
                    if (direction == 1) //LEFT
                    {
                        for (int i = 0; i < _rooms; i++)
                        {
                            if (i == 0 && leftRoomsIndex[0, 0] == null)
                            {
                                Vector2 offset = new Vector2(-1, 0);
                                var itemRoom = new ItemRoom(content, offset);
                                RoomsList.Add(itemRoom);
                                leftRoomsIndex[0, 0] = RoomsList.Count - 1;
                                RoomsList[(int)leftRoomsIndex[0, 0]].GenerateDoor("right", RoomsList[0]);
                                RoomsList[0].GenerateDoor("left", RoomsList[(int)leftRoomsIndex[0, 0]]);
                                roomCount++;
                                break;
                            }
                            else if (leftRoomsIndex[i, 0] != null)
                            {
                                if (rnd.Next(1, 11) <= _branchRoomChance)
                                {
                                    var upOrDown = rnd.Next(1, 3);
                                    if (upOrDown == 1 && leftRoomsIndex[i, 3] == null) //Checks if a room is below
                                    {
                                        Vector2 offset = new Vector2(-(i + 1), 1);
                                        var itemRoom = new ItemRoom(content, offset);
                                        if (i == 0)
                                        {
                                            if (downRoomsIndex[i, 0] != null)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                RoomsList.Add(itemRoom);
                                                leftRoomsIndex[i, 3] = RoomsList.Count - 1;
                                                RoomsList[(int)leftRoomsIndex[i, 3]].GenerateDoor("up", RoomsList[(int)leftRoomsIndex[i, 0]]);
                                                RoomsList[(int)leftRoomsIndex[i, 0]].GenerateDoor("down", RoomsList[(int)leftRoomsIndex[i, 3]]);
                                                roomCount++;
                                                break;
                                            }
                                        }
                                        RoomsList.Add(itemRoom);
                                        leftRoomsIndex[i, 3] = RoomsList.Count - 1;
                                        RoomsList[(int)leftRoomsIndex[i, 3]].GenerateDoor("up", RoomsList[(int)leftRoomsIndex[i, 0]]);
                                        RoomsList[(int)leftRoomsIndex[i, 0]].GenerateDoor("down", RoomsList[(int)leftRoomsIndex[i, 3]]);
                                        roomCount++;
                                        break;
                                    }
                                    else if (upOrDown == 2 && leftRoomsIndex[i, 1] == null) //Checks if a room is above
                                    {
                                        Vector2 offset = new Vector2(-(i + 1), -1);
                                        var itemRoom = new ItemRoom(content, offset);
                                        if (i == 0)
                                        {
                                            if (upRoomsIndex[i, 0] != null)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                RoomsList.Add(itemRoom);
                                                leftRoomsIndex[i, 1] = RoomsList.Count - 1;
                                                RoomsList[(int)leftRoomsIndex[i, 1]].GenerateDoor("down", RoomsList[(int)leftRoomsIndex[i, 0]]);
                                                RoomsList[(int)leftRoomsIndex[i, 0]].GenerateDoor("up", RoomsList[(int)leftRoomsIndex[i, 1]]);
                                                roomCount++;
                                                break;
                                            }
                                        }
                                        RoomsList.Add(itemRoom);
                                        leftRoomsIndex[i, 1] = RoomsList.Count - 1;
                                        RoomsList[(int)leftRoomsIndex[i, 1]].GenerateDoor("down", RoomsList[(int)leftRoomsIndex[i, 0]]);
                                        RoomsList[(int)leftRoomsIndex[i, 0]].GenerateDoor("up", RoomsList[(int)leftRoomsIndex[i, 1]]);
                                        roomCount++;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                Vector2 offset = new Vector2(-(i + 1), 0);
                                var itemRoom = new ItemRoom(content, offset);
                                RoomsList.Add(itemRoom);
                                leftRoomsIndex[i, 0] = RoomsList.Count - 1;
                                RoomsList[(int)leftRoomsIndex[i, 0]].GenerateDoor("right", RoomsList[(int)leftRoomsIndex[i - 1, 0]]);
                                RoomsList[(int)leftRoomsIndex[i - 1, 0]].GenerateDoor("left", RoomsList[(int)leftRoomsIndex[i, 0]]);
                                roomCount++;
                                break;
                            }
                        }

                    }

                    else if (direction == 2) // UP
                    {
                        for (int i = 0; i < _rooms; i++)
                        {
                            if (i == 0 && upRoomsIndex[0, 1] == null)
                            {
                                Vector2 offset = new Vector2(0, -1);
                                var itemRoom = new ItemRoom(content, offset);
                                RoomsList.Add(itemRoom);
                                upRoomsIndex[0, 1] = RoomsList.Count - 1;
                                RoomsList[(int)upRoomsIndex[0, 1]].GenerateDoor("down", RoomsList[0]);
                                RoomsList[0].GenerateDoor("up", RoomsList[(int)upRoomsIndex[0, 1]]);
                                roomCount++;
                                break;
                            }
                            else if (upRoomsIndex[i, 1] != null)
                            {
                                if (i < upRoomsIndex.Length - 1)
                                {
                                    if (rnd.Next(1, 11) <= _branchRoomChance)
                                    {
                                        var leftOrRight = rnd.Next(1, 3);
                                        if (leftOrRight == 1 && upRoomsIndex[i, 0] == null) //Checks if a room is to the left
                                        {
                                            Vector2 offset = new Vector2(-1, -(i + 1));
                                            var itemRoom = new ItemRoom(content, offset);
                                            if (i == 0)
                                            {
                                                if (leftRoomsIndex[i, 1] != null)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    RoomsList.Add(itemRoom);
                                                    upRoomsIndex[i, 0] = RoomsList.Count - 1;
                                                    RoomsList[(int)upRoomsIndex[i, 0]].GenerateDoor("right", RoomsList[(int)upRoomsIndex[i, 1]]);
                                                    RoomsList[(int)upRoomsIndex[i, 1]].GenerateDoor("left", RoomsList[(int)upRoomsIndex[i, 0]]);
                                                    roomCount++;
                                                    break;
                                                }
                                            }
                                            RoomsList.Add(itemRoom);
                                            upRoomsIndex[i, 0] = RoomsList.Count - 1;
                                            RoomsList[(int)upRoomsIndex[i, 0]].GenerateDoor("right", RoomsList[(int)upRoomsIndex[i, 1]]);
                                            RoomsList[(int)upRoomsIndex[i, 1]].GenerateDoor("left", RoomsList[(int)upRoomsIndex[i, 0]]);
                                            roomCount++;
                                            break;
                                        }
                                        else if (leftOrRight == 2 && upRoomsIndex[i, 2] == null) //Checks if a room is to the right
                                        {
                                            Vector2 offset = new Vector2(1, -(i + 1));
                                            var itemRoom = new ItemRoom(content, offset);
                                            if (i == 0)
                                            {
                                                if (rightRoomsIndex[i, 1] != null)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    RoomsList.Add(itemRoom);
                                                    upRoomsIndex[i, 2] = RoomsList.Count - 1;
                                                    RoomsList[(int)upRoomsIndex[i, 2]].GenerateDoor("left", RoomsList[(int)upRoomsIndex[i, 1]]);
                                                    RoomsList[(int)upRoomsIndex[i, 1]].GenerateDoor("right", RoomsList[(int)upRoomsIndex[i, 2]]);
                                                    roomCount++;
                                                    break;
                                                }
                                            }
                                            RoomsList.Add(itemRoom);
                                            upRoomsIndex[i, 2] = RoomsList.Count - 1;
                                            RoomsList[(int)upRoomsIndex[i, 2]].GenerateDoor("left", RoomsList[(int)upRoomsIndex[i, 1]]);
                                            RoomsList[(int)upRoomsIndex[i, 1]].GenerateDoor("right", RoomsList[(int)upRoomsIndex[i, 2]]);
                                            roomCount++;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Vector2 offset = new Vector2(0, -(i + 1));
                                var itemRoom = new ItemRoom(content, offset);
                                RoomsList.Add(itemRoom);
                                upRoomsIndex[i, 1] = RoomsList.Count - 1;
                                RoomsList[(int)upRoomsIndex[i, 1]].GenerateDoor("down", RoomsList[(int)upRoomsIndex[i - 1, 1]]);
                                RoomsList[(int)upRoomsIndex[i - 1, 1]].GenerateDoor("up", RoomsList[(int)upRoomsIndex[i, 1]]);
                                roomCount++;
                                break;
                            }
                        }

                    }

                    else if (direction == 3) //RIGHT
                    {
                        for (int i = 0; i < _rooms; i++)
                        {
                            if (i == 0 && rightRoomsIndex[0, 2] == null)
                            {
                                Vector2 offset = new Vector2(1, 0);
                                var itemRoom = new ItemRoom(content, offset);
                                RoomsList.Add(itemRoom);
                                rightRoomsIndex[0, 2] = RoomsList.Count - 1;
                                RoomsList[(int)rightRoomsIndex[0, 2]].GenerateDoor("left", RoomsList[0]);
                                RoomsList[0].GenerateDoor("right", RoomsList[(int)rightRoomsIndex[0, 2]]);
                                roomCount++;
                                break;
                            }
                            else if (rightRoomsIndex[i, 2] != null)
                            {
                                if (i < rightRoomsIndex.Length - 1)
                                {
                                    if (rnd.Next(1, 11) <= _branchRoomChance)
                                    {
                                        var upOrDown = rnd.Next(1, 3);
                                        if (upOrDown == 1 && rightRoomsIndex[i, 3] == null) //Checks if a room is below
                                        {
                                            Vector2 offset = new Vector2((i + 1), 1);
                                            var itemRoom = new ItemRoom(content, offset);
                                            if (i == 0)
                                            {
                                                if (downRoomsIndex[i, 2] != null)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    RoomsList.Add(itemRoom);
                                                    rightRoomsIndex[i, 3] = RoomsList.Count - 1;
                                                    RoomsList[(int)rightRoomsIndex[i, 3]].GenerateDoor("up", RoomsList[(int)rightRoomsIndex[i, 2]]);
                                                    RoomsList[(int)rightRoomsIndex[i, 2]].GenerateDoor("down", RoomsList[(int)rightRoomsIndex[i, 3]]);
                                                    roomCount++;
                                                    break;
                                                }
                                            }
                                            RoomsList.Add(itemRoom);
                                            rightRoomsIndex[i, 3] = RoomsList.Count - 1;
                                            RoomsList[(int)rightRoomsIndex[i, 3]].GenerateDoor("up", RoomsList[(int)rightRoomsIndex[i, 2]]);
                                            RoomsList[(int)rightRoomsIndex[i, 2]].GenerateDoor("down", RoomsList[(int)rightRoomsIndex[i, 3]]);
                                            roomCount++;
                                            break;
                                        }
                                        else if (upOrDown == 2 && rightRoomsIndex[i, 1] == null) //Checks if a room is above
                                        {
                                            Vector2 offset = new Vector2((i + 1), -1);
                                            var itemRoom = new ItemRoom(content, offset);
                                            if (i == 0)
                                            {
                                                if (upRoomsIndex[i, 2] != null)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    RoomsList.Add(itemRoom);
                                                    rightRoomsIndex[i, 1] = RoomsList.Count - 1;
                                                    RoomsList[(int)rightRoomsIndex[i, 1]].GenerateDoor("down", RoomsList[(int)rightRoomsIndex[i, 2]]);
                                                    RoomsList[(int)rightRoomsIndex[i, 2]].GenerateDoor("up", RoomsList[(int)rightRoomsIndex[i, 1]]);
                                                    roomCount++;
                                                    break;
                                                }
                                            }
                                            RoomsList.Add(itemRoom);
                                            rightRoomsIndex[i, 1] = RoomsList.Count - 1;
                                            RoomsList[(int)rightRoomsIndex[i, 1]].GenerateDoor("down", RoomsList[(int)rightRoomsIndex[i, 2]]);
                                            RoomsList[(int)rightRoomsIndex[i, 2]].GenerateDoor("up", RoomsList[(int)rightRoomsIndex[i, 1]]);
                                            roomCount++;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Vector2 offset = new Vector2((i + 1), 0);
                                var itemRoom = new ItemRoom(content, offset);
                                RoomsList.Add(itemRoom);
                                rightRoomsIndex[i, 2] = RoomsList.Count - 1;
                                RoomsList[(int)rightRoomsIndex[i, 2]].GenerateDoor("left", RoomsList[(int)rightRoomsIndex[i - 1, 2]]);
                                RoomsList[(int)rightRoomsIndex[i - 1, 2]].GenerateDoor("right", RoomsList[(int)rightRoomsIndex[i, 2]]);
                                roomCount++;
                                break;
                            }
                        }
                    }

                    else if (direction == 4) //DOWN
                    {
                        for (int i = 0; i < _rooms; i++)
                        {
                            if (i == 0 && downRoomsIndex[0, 3] == null)
                            {
                                Vector2 offset = new Vector2(0, 1);
                                var itemRoom = new ItemRoom(content, offset);
                                RoomsList.Add(itemRoom);
                                downRoomsIndex[0, 3] = RoomsList.Count - 1;
                                RoomsList[(int)downRoomsIndex[0, 3]].GenerateDoor("up", RoomsList[0]);
                                RoomsList[0].GenerateDoor("down", RoomsList[(int)downRoomsIndex[0, 3]]);
                                roomCount++;
                                break;
                            }
                            else if (downRoomsIndex[i, 3] != null)
                            {
                                if (i < downRoomsIndex.Length - 1)
                                {
                                    if (rnd.Next(1, 11) <= _branchRoomChance)
                                    {
                                        var leftOrRight = rnd.Next(1, 3);
                                        if (leftOrRight == 1 && downRoomsIndex[i, 0] == null) //Checks if a room is to the left
                                        {
                                            Vector2 offset = new Vector2(-1, (i + 1));
                                            var itemRoom = new ItemRoom(content, offset);
                                            if (i == 0)
                                            {
                                                if (leftRoomsIndex[i, 3] != null)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    RoomsList.Add(itemRoom);
                                                    downRoomsIndex[i, 0] = RoomsList.Count - 1;
                                                    RoomsList[(int)downRoomsIndex[i, 0]].GenerateDoor("right", RoomsList[(int)downRoomsIndex[i, 3]]);
                                                    RoomsList[(int)downRoomsIndex[i, 3]].GenerateDoor("left", RoomsList[(int)downRoomsIndex[i, 0]]);
                                                    roomCount++;
                                                    break;
                                                }
                                            }
                                            RoomsList.Add(itemRoom);
                                            downRoomsIndex[i, 0] = RoomsList.Count - 1;
                                            RoomsList[(int)downRoomsIndex[i, 0]].GenerateDoor("right", RoomsList[(int)downRoomsIndex[i, 3]]);
                                            RoomsList[(int)downRoomsIndex[i, 3]].GenerateDoor("left", RoomsList[(int)downRoomsIndex[i, 0]]);
                                            roomCount++;
                                            break;
                                        }
                                        else if (leftOrRight == 2 && downRoomsIndex[i, 2] == null) //Checks if a room is to the right
                                        {
                                            Vector2 offset = new Vector2(1, (i + 1));
                                            var itemRoom = new ItemRoom(content, offset);
                                            if (i == 0)
                                            {
                                                if (rightRoomsIndex[i, 3] != null)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    RoomsList.Add(itemRoom);
                                                    downRoomsIndex[i, 2] = RoomsList.Count - 1;
                                                    RoomsList[(int)downRoomsIndex[i, 2]].GenerateDoor("left", RoomsList[(int)downRoomsIndex[i, 3]]);
                                                    RoomsList[(int)downRoomsIndex[i, 3]].GenerateDoor("right", RoomsList[(int)downRoomsIndex[i, 2]]);
                                                    roomCount++;
                                                    break;
                                                }
                                            }
                                            RoomsList.Add(itemRoom);
                                            downRoomsIndex[i, 2] = RoomsList.Count - 1;
                                            RoomsList[(int)downRoomsIndex[i, 2]].GenerateDoor("left", RoomsList[(int)downRoomsIndex[i, 3]]);
                                            RoomsList[(int)downRoomsIndex[i, 3]].GenerateDoor("right", RoomsList[(int)downRoomsIndex[i, 2]]);
                                            roomCount++;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Vector2 offset = new Vector2(0, (i + 1));
                                var itemRoom = new ItemRoom(content, offset);
                                RoomsList.Add(itemRoom);
                                downRoomsIndex[i, 3] = RoomsList.Count - 1;
                                RoomsList[(int)downRoomsIndex[i, 3]].GenerateDoor("up", RoomsList[(int)downRoomsIndex[i - 1, 3]]);
                                RoomsList[(int)downRoomsIndex[i - 1, 3]].GenerateDoor("down", RoomsList[(int)downRoomsIndex[i, 3]]);
                                roomCount++;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    //Add code for Boss rooms
                    roomCount++;
                }
            }
        }
    }
}


using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using static RtanTextRpg.Program;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RtanTextRpg
{
    struct ObjectInfo
    {
        public int Lv;
        public string Name;
        public string Job;
        public int Att;
        public int Def;
        public int Hp;
        public int Gold;
        public int Number;
        public string Descrption;
        public bool equip;
    }
    internal class Program
    {
        public static void Start()
        {
            Console.WriteLine("===================================================");
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine("===================================================");
            Console.WriteLine("1.상태보기");
            Console.WriteLine("2.인벤토리");
            Console.WriteLine("3.상점");
            Console.WriteLine("4.종료");

            Console.Write("원하시는 행동을 입력해주세요 :");
            int select = int.Parse(Console.ReadLine());

            switch (select)
            {
                case 1:
                    status();
                    break;
                case 2:
                    Inventory();
                    break;
                case 3:
                    ShopMain();
                    break;
                case 4:
                    return;
                default:
                    Console.WriteLine("잘못된 입력입니다");
                    Console.WriteLine("초기상태로 돌아갑니다");
                    Start();
                    break;
            }
        }

        public static void Inventory()
        {
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");

            Console.WriteLine("[아이템 목록]");


            Console.WriteLine("1. 장착관리");
            Console.WriteLine("0. 나가기");

            Console.Write("원하시는 행동을 입력해주세요 :");
            int select = int.Parse(Console.ReadLine());

            switch (select)
            {
                case 0:
                    Start();
                    break;
                case 1:
                    Inventory_Equip();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다");
                    Console.WriteLine("다시 입력해주세요");
                    Inventory();
                    break;
            }


        }
        public static void Inventory_Equip()
        {
            Console.WriteLine("인벤토리 - 장착관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");

            PlayerInventory inventory = new PlayerInventory();
            int itemNumber = inventory.DisplayInventory();
            if (itemNumber == 0)
            {
                return; // 0을 선택하면 나가기
            }
            else
            {
                inventory.EquipItem(itemNumber); // 선택한 아이템을 장착
            }


        }

        public class Item
        {

            //public static int Id = 0;
            public int Number;
            public string Name;
            public int Att;
            public int Def;
            public string Description;
            public bool Equip;

            public Item(int number, string name, int att, int def, string description, bool Equip)
            {
                Number = number;
                Name = name;
                Att = att;
                Def = def;
                Description = description;
                this.Equip = Equip;
            }
        }
        internal class PlayerInventory
        {
            public List<Item> items;
            private Item equippedItem;

            public PlayerInventory()
            {
                items = new List<Item>();
                InventoryList();
            }

            public void InventoryList()
            {
                items.Add(new Item(1, "무쇠 갑옷", 0, 5, "무쇠로 만들어져 튼튼한 갑옷입니다.", false));
                items.Add(new Item(2, "스파르타의 창", 7, 0, "스파르타의 전사들이 사용했다는 전설의 창입니다.", false));
                items.Add(new Item(3, "낡은 검", 2, 0, "쉽게 볼 수 있는 낡은 검 입니다.", false));
                // 추가를 원하면  items.Add(new Item(4, "낡은 검", 0, 5, "쉽게 볼 수 있는 낡은 검 입니다.,false")); number값을 변경해준다 (겹치면 안돌아감)
            }

            public int DisplayInventory()
            {
                Console.WriteLine("[아이템 목록]");
                // 아이템을 받아온다                      
                foreach (var item in items)
                {
                    //Console.WriteLine(item.Name);
                    //Console.WriteLine(item.Equip);
                    string EquipState = item.Equip ? "[E]" : " ";
                    Console.WriteLine($"{EquipState} \t|{item.Number}. {item.Name} | 공격력 {item.Att} | 방어력 {item.Def} | {item.Description}");
                }
                Console.WriteLine("");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("");
                Console.WriteLine("장착을 원하시는 장비 숫자를 입력해주세요");
                int itemNumber = int.Parse(Console.ReadLine());
                switch (itemNumber)
                {
                    case 0:
                        Start();
                        return 0;
                    default:
                        return itemNumber;
                }
            }
            public void EquipItem(int itemNumber)
            {
                Item SelectedEquip = FindItem(itemNumber);
                if (SelectedEquip != null)
                {
                    //이미 장착이 되어있으면 해제 
                    if (equippedItem != null)
                    {
                        Console.WriteLine($"장착 해제: {equippedItem.Name}");
                        equippedItem.Equip = false;
                        equippedItem = null;
                        Inventory_Equip();
                    }

                    // 선택한 아이템을 장착.
                    equippedItem = SelectedEquip;
                    Console.WriteLine($"장착 완료: {equippedItem.Name}");
                    equippedItem.Equip = true;
                    Addstatus(equippedItem);
                }
                else
                {
                    Console.WriteLine("해당 아이템을 찾을 수 없습니다.");
                    Inventory_Equip();
                }
            }
            public Item FindItem(int number)
            {
                return items.Find(item => item.Number == number);
            }
        }

        public class Shop
        {
            public int Number;
            public string Name;
            public int Att;
            public int Def;
            public int Gold;
            public string Description;
            public bool Check;

            public Shop(int number, string name, int att, int def, int gold, string description, bool check)
            {
                Number = number;
                Name = name;
                Att = att;
                Def = def;
                Description = description;
                Check = check;
            }
        }

        internal class ShopView
        {
            public List<Shop> Shops;

            public ShopView()
            {
                Shops = new List<Shop>();
                ShopList();
            }
            public void ShopList()
            {
                Shops.Add(new Shop(1, "수련자 갑옷", 0, 5, 1000, "수련에 도움을 주는 갑옷입니다.", false));
                Shops.Add(new Shop(2, "무쇠갑옷", 0, 9, 1500, "무쇠로 만들어져 튼튼한 갑옷입니다.", false));
                Shops.Add(new Shop(3, "스파르타의 갑옷", 0, 15, 3500, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", false));
                Shops.Add(new Shop(4, "낡은 검", 2, 0, 600, "쉽게 볼 수 있는 낡은 검 입니다.", false));
                Shops.Add(new Shop(5, "청동 도끼", 5, 0, 1500, "어디선가 사용됐던거 같은 도끼입니다.", false));
                Shops.Add(new Shop(6, "스파르타의 창", 7, 0, 2500, "스파르타의 전사들이 사용했다는 전설의 창입니다.", false));
            }
            public int ShopViewList()
            {
                Console.WriteLine("[아이템 목록]");
                // 아이템을 받아온다                      
                foreach (var Shop in Shops)
                {
                    //Console.WriteLine($"- \t|{item.Number}. {item.Name} | 공격력 {item.Att} | 방어력 {item.Def} | {item.Description}");
                    Console.WriteLine($"- {Shop.Name} | 공격력 + {Shop.Att} | 방어력 + {Shop.Def} | {Shop.Description} {Shop.Gold}");
                    Console.WriteLine("");
                    Console.WriteLine("0. 나가기");
                    Console.WriteLine("");
                    Console.WriteLine("1. 아이템 구매");
                    int select = int.Parse(Console.ReadLine());

                    if (select == 0)
                    {
                        Start();
                        return 0;
                    }
                    else if (select == 1)
                    {
                        ShopBuy();
                        return 0;
                    }
                }
                return 0;
            }
        }

        internal class ShopBuy
        {
            public List<Shop> Shops;

            public ShopBuy()
            {
                Shops = new List<Shop>();
                ShopList();
            }
            public void ShopList()
            {
                Shops.Add(new Shop(1, "수련자 갑옷", 0, 5, 1000, "수련에 도움을 주는 갑옷입니다.", false));
                Shops.Add(new Shop(2, "무쇠갑옷", 0, 9, 1500, "무쇠로 만들어져 튼튼한 갑옷입니다.", false));
                Shops.Add(new Shop(3, "스파르타의 갑옷", 0, 15, 3500, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", false));
                Shops.Add(new Shop(4, "낡은 검", 2, 0, 600, "쉽게 볼 수 있는 낡은 검 입니다.", false));
                Shops.Add(new Shop(5, "청동 도끼", 5, 0, 1500, "어디선가 사용됐던거 같은 도끼입니다.", false));
                Shops.Add(new Shop(6, "스파르타의 창", 7, 0, 2500, "스파르타의 전사들이 사용했다는 전설의 창입니다.", false));
            }
            public int ShopBuyList()
            {
                Console.WriteLine("[아이템 목록]");
                // 아이템을 받아온다                      
                foreach (var Shop in Shops)
                {
                    Console.WriteLine($"-{Shop.Number} {Shop.Name} | 공격력 + {Shop.Att} | 방어력 + {Shop.Def} | {Shop.Description} {Shop.Gold}");
                    Console.WriteLine("");
                    Console.WriteLine("0. 나가기");
                    Console.WriteLine("");
                    int select = int.Parse(Console.ReadLine());

                    if (select == 0)
                    {
                        Start();
                        return 0;
                    }
                    else if (select == 1)
                    {
                        //ShopBuy();
                        return 0;
                    }
                }
                return 0;
            }
        }
        public static void ShopMain()
        {
            ObjectInfo playerInfo;
            playerInfo.Gold = 800;

            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");

            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{playerInfo.Gold} G");

            ShopView shop = new ShopView();
            int select = shop.ShopViewList();




        }

        public static void status()
        {
            ObjectInfo playerInfo;
            playerInfo.Lv = 01;
            playerInfo.Name = "Chad";
            playerInfo.Job = "전사";
            playerInfo.Att = 10;
            playerInfo.Def = 5;
            playerInfo.Hp = 100;
            playerInfo.Gold = 1500;
            int pulsAtt = 0;
            int pulsDef = 0;


            Console.WriteLine("상태보기");

            Console.WriteLine("캐릭터의 정보가 표시됩니다.");

            Console.WriteLine("Lv: " + playerInfo.Lv);
            Console.WriteLine(playerInfo.Name + "(" + playerInfo.Job + ")");
            Console.WriteLine("공격력: " + playerInfo.Att + "(" + pulsAtt + ")");
            Console.WriteLine("방어력: " + playerInfo.Def + "(" + pulsDef + ")");
            Console.WriteLine("체력: " + playerInfo.Hp);
            Console.WriteLine("Gold: " + playerInfo.Gold + "G");



            Console.WriteLine("0. 로비로 이동");

            Console.Write("원하시는 행동을 입력해주세요 :");
            int select = int.Parse(Console.ReadLine());
            if (select == 0)
            {
                Start();
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다");
                Console.WriteLine("다시 입력해주세요");
                Start();
            }
        }

        public static void Addstatus(Item equippedItem)
        {
            ObjectInfo playerInfo;
            playerInfo.Lv = 01;
            playerInfo.Name = "Chad";
            playerInfo.Job = "전사";
            playerInfo.Att = 10;
            playerInfo.Def = 5;
            playerInfo.Hp = 100;
            playerInfo.Gold = 1500;
            int pulsAtt = 0;
            int pulsDef = 0;
            if (equippedItem != null)
            {
                pulsAtt += equippedItem.Att;
                pulsDef += equippedItem.Def;
            }

            Console.WriteLine("상태보기");

            Console.WriteLine("캐릭터의 정보가 표시됩니다.");

            Console.WriteLine("Lv: " + playerInfo.Lv);
            Console.WriteLine(playerInfo.Name + "(" + playerInfo.Job + ")");
            Console.WriteLine("공격력: " + playerInfo.Att + "+" + "(" + pulsAtt + ")");
            Console.WriteLine("방어력: " + playerInfo.Def + "+" + "(" + pulsDef + ")");
            Console.WriteLine("체력: " + playerInfo.Hp);
            Console.WriteLine("Gold: " + playerInfo.Gold + "G");



            Console.WriteLine("0. 로비로 이동");

            Console.Write("원하시는 행동을 입력해주세요 :");
            int select = int.Parse(Console.ReadLine());
            if (select == 0)
            {
                Start();
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다");
                Console.WriteLine("다시 입력해주세요");
                Start();
            }
        }


        static void Main(string[] args)
        {
            Start();

        }
    }
}

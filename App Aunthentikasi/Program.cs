using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace AppAuth
{

    class Program
    {
        public string ulang_menu = "y";
        public string ulang = "y";
        public string handler;

        string firstname = "";
        string lastname = "";

        static void Main(string[] args)
        {
            string[] first_name = new string[50];
            string[] last_name = new string[50];
            string[] username = new string[50];
            string[] password = new string[50];

            Program p = new Program();
            do
            {
                Console.Clear();
                Console.WriteLine("=============BASIC AUNTHENTICATION=============");
                Console.WriteLine("1. Create User");
                Console.WriteLine("2. Show User");
                Console.WriteLine("3. Search User");
                Console.WriteLine("4. Login User");
                Console.WriteLine("5. Exit User");
                Console.Write("Input : ");
                int input_menu = Convert.ToInt32(Console.ReadLine());
                switch (input_menu)
                {
                    case 1:
                        Console.Clear();

                        //Versi 2
                        Console.Write("First Name : ");
                        string input_firstname = Console.ReadLine();
                        if (ValidateLengthName(input_firstname))
                        {
                            p.firstname = input_firstname;
                        }
                        else
                        {
                            Console.WriteLine("Name has to be at least consisting 2 characters or more.");
                            Console.Write("Pilih : ");
                            string i_firstname = Console.ReadLine();
                            p.firstname = i_firstname;
                        }

                        Console.Write("Last Name : ");
                        string input_lastname = Console.ReadLine();
                        if (ValidateLengthName(input_lastname))
                        {
                            p.lastname = input_lastname;
                        }
                        else
                        {
                            Console.WriteLine("Name has to be at least consisting 2 characters or more.");
                            Console.Write("Pilih : ");
                            string i_lastname = Console.ReadLine();
                            p.lastname = i_lastname;
                        }
                        do
                        {
                            Console.Write("Password : ");
                            string input_password = Console.ReadLine();
                            if (ValidateLengthPass(input_password))
                            {
                                if (ValidatePassword(input_password))
                                {
                                    if (ValidateLengthName(p.firstname) & ValidateLengthName(p.lastname))
                                    {
                                        Console.WriteLine(Insert(first_name, last_name, password, username,
                                            p.firstname, p.lastname, input_password));
                                        EventHandlerKeypressed();
                                        p.ulang = "n";
                                    }
                                    else { MessageError(); EventHandlerKeypressed(); p.ulang = "n"; }
                                }
                                else
                                {
                                    Console.WriteLine("with at least one Capital letter, at least one lower case letter and at least one number.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Password must have at least 8 characters.");
                            }
                        } while (p.ulang == "y");
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("==SHOW USER==");
                        Console.WriteLine("====================");
                        for (int i = 0; i < username.Length; i++)
                        {
                            if (first_name[i] != null) // by username array
                            {
                                Console.WriteLine($"ID \t: {i + 1}");
                                Console.WriteLine($"Name \t: {first_name[i]} {last_name[i]}");
                                Console.WriteLine($"Username: {username[i]}");
                                Console.WriteLine($"Password: {password[i]}");
                                Console.WriteLine("====================");
                            }
                        }
                        Console.WriteLine("Menu");
                        Console.WriteLine("1.Edit User");
                        Console.WriteLine("2.Delete User");
                        Console.WriteLine("3.Back ");
                        Console.Write("Input : ");
                        string pilih = Console.ReadLine();
                        switch (pilih)
                        {
                            case "1":

                                Console.Write("ID yang ingin diubah : ");
                                string id = Console.ReadLine();
                                int num = -1;
                                if (!int.TryParse(id, out num))
                                {
                                    Console.Write("ERROR");
                                }
                                else
                                {
                                    if (cekId(id, username))
                                    {
                                        Console.Write("First Name : ");
                                        string first_edit = Console.ReadLine();
                                        Console.Write("Last Name : ");
                                        string last_edit = Console.ReadLine();
                                        Console.Write("Password : ");
                                        string pass_edit = Console.ReadLine();
                                        Console.WriteLine(Edit(first_name, last_name, username, password,
                                            first_edit, last_edit, pass_edit, id));

                                    }
                                    else { Console.WriteLine("ID tidak ditemukan"); }

                                }
                                EventHandlerKeypressed();
                                break;
                            case "2":
                                Console.WriteLine("ID yang ingin Delete : ");
                                string id_delete = Console.ReadLine();

                                if (!int.TryParse(id_delete, out num))
                                {
                                    Console.Write("ERROR");
                                }
                                else
                                {
                                    Console.WriteLine(DeleteId(first_name,last_name,username,password, id_delete));
                                }
                                EventHandlerKeypressed();
                                break;
                            case "3":
                                break;
                            default:
                                break;
                        }
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("==Cari Akun==");
                        string cari = Console.ReadLine();
                        for (int i = 0; i < first_name.Length; i++)
                        {
                            if (first_name[i] != null)
                            {
                                if (first_name[i].Contains(cari) | last_name.Contains(cari))
                                {
                                    Console.WriteLine("====================");
                                    Console.WriteLine($"ID \t: {i + 1}");
                                    Console.WriteLine($"Name \t: {first_name[i]} {last_name[i]}");
                                    Console.WriteLine($"Username: {username[i]}");
                                    Console.WriteLine($"Password: {password[i]}");
                                    Console.WriteLine("====================");
                                }
                            }
                        }
                        EventHandlerKeypressed();
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine("==LOGIN==");
                        Console.Write("USERNAME : ");
                        string login_user = Console.ReadLine();
                        Console.Write("PASSWORD : ");
                        string login_pass = Console.ReadLine();
                        Console.WriteLine(LoginMessage(LoginValidate(login_user, login_pass, username, password)));
                        EventHandlerKeypressed();
                        break;
                    case 5:
                        Exit();
                        break;
                    default:
                        MessageError();
                        EventHandlerKeypressed();
                        Main(args);
                        break;
                }
            } while (p.ulang_menu == "y");
        }

        static void Exit()
        {
            Environment.Exit(0);
        }
        static void MessageError()
        {
            Console.WriteLine("ERROR : Input Not Valid");
        }
        static void EventHandlerKeypressed()
        {
            Console.WriteLine("Press any key...");
            Console.ReadKey(true);
            Console.Clear();
        }
        static bool LoginValidate(string login_user, string login_pass,
            string[] username, string[] password)
        {
            bool cek = false;
            for (int i = 0; i < username.Length; i++)
            {
                if (username[0] != null)
                {
                    if (username[i] == login_user & password[i] == login_pass)
                    {
                        cek = true;
                    }

                }
                else { cek = false; }
            }
            return cek;
        }
        static string LoginMessage(bool input)
        {
            if (input == true)
            {
                return "MESSAGE : LOGIN BERHASIL !!";
            }
            else
            {
                return "Username dan Password tidak ditemukan.";
            }
        }
        static int GetArrayIndex(string[] name)
        {
            int arr = 0;
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] != null)
                { arr += 1; }
            }
            return arr;
        }

        //Versi 2
        static bool ValidateLengthName(string input)
        {
            if (input.Length > 1)
            { return true; }
            else { return false; }
        }
        static bool ValidateLengthPass(string input)
        {
            if (input.Length > 7)
            { return true; }
            else { return false; }
        }
        static bool ValidatePassword(string input_password)
        {
            if (input_password.Any(char.IsUpper)
                            & input_password.Any(char.IsLower) & input_password.Any(char.IsDigit))
            { return true; }
            else { return false; }
        }
        static string Insert(string[] first, string[] last, string[] pass, string[] username,
            string firstname, string lastname, string pw)
        {
            string first_name = GetTwoChar(firstname);
            string last_name = GetTwoChar(lastname);
            if (CheckUsername(username, first_name, last_name))
            {
                first[GetArrayIndex(first)] = firstname;
                last[GetArrayIndex(last)] = lastname;
                username[GetArrayIndex(username)] = first_name + last_name;
                pass[GetArrayIndex(pass)] = pw;
                return $"User Success to Created!!! {GetArrayIndex(first)}";
            }
            else
            {
                return "Create failure,Username already exists!!!";
            }

        }
        static string GetTwoChar(string name)
        {
            string karakter = "";
            if (name != null)
            {
                for (int i = 0; i < name.Length; i++)
                {
                    if (i == 0 | i == 1)
                    {
                        karakter += name[i];
                    }
                    else { break; }
                }
            }
            return karakter;
        }
        static bool CheckUsername(string[] username, string first_name, string last_name)
        {
            bool cek = true;
            for (int i = 0; i < username.Length; i++)
            {
                if (username[0] != null)
                {
                    if (username[i] == first_name + last_name)
                    {
                        cek = false;
                    }

                }
                else { cek = true; }
            }
            return cek;
        }

        static string Edit(string[] first, string[] last, string[] username, string[] pass,
            string firstname, string lastname ,string pw, string id)
        {
            string msg = "";
            if (ValidateLengthPass(pw) & ValidatePassword(pw) &
                ValidateLengthName(firstname) & ValidateLengthName(lastname))
            {
                int ids = Int32.Parse(id);
                string first_name = GetTwoChar(firstname);
                string last_name = GetTwoChar(lastname);
                if (CheckUsername(username, first_name, last_name))
                {
                    first[ids - 1] = firstname;
                    last[ids - 1] = lastname;
                    username[ids - 1] = first_name + last_name;
                    pass[ids - 1] = pw;
                    msg = "User Success to Created!!!";
                }
                else
                {
                    msg = "Create failure,Username already exists!!!";
                }
            }
            else
            {
                msg = "ERROR : Input Not Valid";
            }

            return msg;
        }

        static bool cekId(string id, string[] username)
        {
            int ids = Int32.Parse(id);
            bool cek = false;
            for (int i = 0; i < username.Length; i++)
            {
                if (username[i] != null)
                {
                    if (i == (ids - 1))
                    {
                        cek = true;
                        break;
                    }
                }
                else { cek = false; break; }
            }

            return cek;
        }

        static string DeleteId(string[] first, string[] last, string[] username, string[] pass, string id) {
            int i = 0;
            int flags = 0;
            int index = 0;
            int ids = Int32.Parse(id);
            string msg = "";
            if (GetArrayIndex(username) != 0)
            {
                for (i = 0; i < GetArrayIndex(username); i++)
                {
                    if (i == ids-1)
                    {
                        flags = 1;
                        index = i;
                        break;
                    }
                }
                if (flags == 1)
                {
                    index = i;
                    for (i = index; i < GetArrayIndex(username); i++)
                    {
                        first[i] = first[i + 1];
                        last[i] = last[i + 1];
                        username[i] = username[i + 1];
                        pass[i] = pass[i + 1];

                        if (i == GetArrayIndex(username))
                        {
                            first[i] = null;
                            last[i] = null;
                            username[i] = null;
                            pass[i] = null;
                        }
                    }
                    msg = $"ID Berhasil di Delete {GetArrayIndex(username)} : {index}";
                }
                else { msg = $" gagal flag"; }
                
                
            }
            else { msg = "ID tidak ditemukan"; }
            
            return msg;
        }

        //Versi 1

        /*static string ValidateName(string name, string[] first_name, string[] last_name, int akses)
        {
            Program p = new Program();
            if (name.Length > 1 & akses == 0 & last_name == null)

            {
                if (GetArrayIndex(first_name) != 0)
                {
                    first_name[GetArrayIndex(first_name) + 1] = name;
                    return "n1";
                }
                else
                {
                    first_name[0] = name;
                    return "n0";
                }

            }
            else if (name.Length > 1 & akses == 1 & first_name == null)
            {
                if (GetArrayIndex(last_name) != 0)
                {
                    last_name[GetArrayIndex(last_name) + 1] = name;
                    return "n1";
                }
                else
                {
                    last_name[0] = name;
                    return "n0";
                }
            }
            else { return "y"; }

        }*/
    }

}


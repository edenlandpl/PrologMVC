using Microsoft.AspNetCore.Mvc;
using Prolog.SQLConnection;
using PrologMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace PrologMVC.Controllers
{
    public class UserController : Controller
    {
        string[] data = new string[999];
        public static string[] zmiennaSesyjna = new string[99];
        public static string[] zmiennaGodzinowa = new string[99];
        public UserLogin userLogin { get; set; }
        int shiftDay = 0;
        public static string loginUserForm;
        static string userFirstname;
        static string lastUserName;
        //private ObservableCollection<UserLogin> userLoginList;

        List<string> userLoginList = new List<string>();
        
        public UserController()
        {
           
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        [HttpGet]
        public IActionResult Logowanie()
        {
            return View();
        }
      
        [HttpPost]
        public IActionResult Logowanie(UserLogin model)
        {
            loginUserForm = model.LoginUser;
            var passwordUserForm = model.PasswordUser;

            DBClass.openConnection();
            //) CAST(DateOfBirth AS date), convert(Date, DateOfBirth, 23) 
            DBClass.sql = "select nameUser, passwordUser, firstNameUser, lastNameUser, phoneNumberUser from users where nameUser = '" + loginUserForm + "' and passwordUser = '" + passwordUserForm + "' ";
            DBClass.cmd.CommandType = CommandType.Text;
            DBClass.cmd.CommandText = DBClass.sql;

            DBClass.da = new SqlDataAdapter(DBClass.cmd);
            DBClass.dt = new DataTable();
            DBClass.da.Fill(DBClass.dt);

            // wyciągamy dane
            int j = 0;
            using (SqlDataReader reader = DBClass.cmd.ExecuteReader())                
            {
                //Debug.WriteLine("Reader długość 01- " + reader.FieldCount);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        
                        for (j = 0; j <= reader.FieldCount - 1; j++) // Looping throw colums
                        {
                            data[j] = reader.GetValue(j).ToString();
                        }
                        zmiennaSesyjna[0] = data[0];
                        zmiennaSesyjna[1] = data[1];
                        zmiennaSesyjna[2] = data[2];
                        zmiennaSesyjna[3] = data[3];
                        zmiennaSesyjna[4] = data[4];
                        userFirstname = data[2];
                        lastUserName = data[3];
                        //userLoginList.Add(data[0]);
                        //userLoginList.Add(data[1]);
                        Debug.WriteLine("Zmienna sesyjna lista - " + zmiennaSesyjna[0] + zmiennaSesyjna[1] + zmiennaSesyjna[2] + zmiennaSesyjna[3] + zmiennaSesyjna[4]);
                        //userLoginList.Add(new UserLogin { LoginUser = (data[0]), PasswordUser = data[1] });
                        //userLoginList.Add(new UserLogin { LoginUser = (data[2]), PasswordUser = data[3] });
                        //Debug.WriteLine("W accouncie01 - " + HttpContext.Session.GetString("Test"));
                        Debug.WriteLine("UserLogin - " + data[0] + data[1]);
                    }
                    DBClass.closeConnection();
                    return RedirectToAction("AccountUser", "User");
                }                
            }
            DBClass.closeConnection();
            ViewBag.Error = "Niepoprawne dane logowania!!!";           
            return View();
            return RedirectToAction("Index", "Home" ,new { userLogin});
            return View("Index", userLogin);
        }
        [HttpPost]
        public IActionResult AccountEdit(UserLogin model)
        {
            String loginUser = Request.Form["LoginUser"];
            String firstNameUser = Request.Form["FirstNameUser"];
            String lastNameUser = Request.Form["LastNameUser"];
            String passwordUser = Request.Form["PasswordUser"];
            String phoneNumberUser = Request.Form["PhoneNumberUser"];
            var loginUserForm = model.LoginUser;
            var passwordUserForm = model.PasswordUser;
            Debug.WriteLine("Z formy - " + loginUserForm + passwordUserForm + firstNameUser + lastNameUser);

            DBClass.openConnection();
            //) CAST(DateOfBirth AS date), convert(Date, DateOfBirth, 23) 
            DBClass.sql = "update users Set nameUser = '" + loginUser + "', firstNameUser = '" + firstNameUser + "' " +
                ",lastNameUser = '" + lastNameUser + "', phoneNumberUser = '" + phoneNumberUser + "' where nameUser = '" + loginUser +"' ";
            //DBClass.sql = "select FirstName, lastName from patients";
            DBClass.cmd.CommandType = CommandType.Text;
            DBClass.cmd.CommandText = DBClass.sql;

            DBClass.da = new SqlDataAdapter(DBClass.cmd);
            DBClass.dt = new DataTable();
            DBClass.da.Fill(DBClass.dt);

            DBClass.closeConnection();

            return RedirectToAction("AccountUser", "User");
        }

        [HttpPost]
        public IActionResult Wylogowanie()
        {
            loginUserForm = null;
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
        
            return View();
        }

        [HttpPost]
        public IActionResult RegisterForm(UserLogin model)
        {
            //String loginUser = Request.Form["LoginUser"];
            //String firstNameUser = Request.Form["FirstNameUser"];
            //String lastNameUser = Request.Form["LastNameUser"];
            //String passwordUser = Request.Form["PasswordUser"];
            //String phoneNumberUser = Request.Form["PhoneNumberUser"];

            return View("KoniecRejestracji");
        }
        public IActionResult AccountUser()
        {            
            //Debug.WriteLine("Zmienna sesyjna - " + zmiennaSesyjna[0] + zmiennaSesyjna[1] + zmiennaSesyjna[2] + zmiennaSesyjna[3]);             
            ViewBag.UserString = zmiennaSesyjna;
            //ViewBag.UserList = userLoginList;
            return View();
        }
        [HttpPost]
        public IActionResult Rezerwacje(ReservationInfo reservationInfo)
        {
            //selecta z rezerwacji, masz lastname therapist i daate
            //zapisac wedle swojego sposobu do viewbagow
            var rezerwacja = new OstatecznaRezerwacja();
            rezerwacja.lastnameTherapis = reservationInfo.lastNameTherapist;
            rezerwacja.dateReservation = reservationInfo.DataOfReservation;

            DBClass.openConnection();
            //) CAST(DateOfBirth AS date), convert(Date, DateOfBirth, 23) 
            //DBClass.sql = "select DataVisitStart from reservations where DataVisitStart = '" + dateReservation + "' order by  DataVisitStart";
            DBClass.sql = "select  TimeStartVisit from reservations where IDTherapist = (select IDTherapist from therapists where lastNameTherapist = '" + rezerwacja.lastnameTherapis + "') and DataVisitStart = '" + rezerwacja.dateReservation + "' order by  TimeStartVisit";
            DBClass.cmd.CommandType = CommandType.Text;
            DBClass.cmd.CommandText = DBClass.sql;

            DBClass.da = new SqlDataAdapter(DBClass.cmd);
            DBClass.dt = new DataTable();
            DBClass.da.Fill(DBClass.dt);

            // wyciągamy dane
            string str = " ";
            int j = 0, i = 1;

            Dictionary<string, bool> dic = new Dictionary<string, bool>
            {
                { "08", false },
                { "09", false },
                { "10", false },
                { "11", false },
                { "12", false },
                { "13", false },
                { "14", false },
                { "15", false }

            };

            using (SqlDataReader reader = DBClass.cmd.ExecuteReader())
            {
                Debug.WriteLine("Reader długość 01- " + reader.FieldCount);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        for (j = 0; j <= reader.FieldCount - 1; j++) // Looping throw colums
                        {
                            data[j] = reader.GetValue(j).ToString();
                        }
                        str = data[0].Substring(0, 2);

                        dic[str] = true;
                    }
                    dic.TryGetValue("08", out bool value08);
                    ViewBag.Hour08 = value08;
                    dic.TryGetValue("09", out bool value09);
                    ViewBag.Hour09 = value09;
                    dic.TryGetValue("10", out bool value10);
                    ViewBag.Hour10 = value10;
                    dic.TryGetValue("11", out bool value11);
                    ViewBag.Hour11 = value11;
                    dic.TryGetValue("12", out bool value12);
                    ViewBag.Hour12 = value12;
                    dic.TryGetValue("13", out bool value13);
                    ViewBag.Hour13 = value13;
                    dic.TryGetValue("14", out bool value14);
                    ViewBag.Hour14 = value14;
                    dic.TryGetValue("15", out bool value15);
                    ViewBag.Hour15 = value15;
                    //ViewBag.UserString = dic;
                    //ViewBag.UserStringHour = zmiennaGodzinowa;
                    //DBClass.closeConnection();
                    //return View(rezerwacja);
                    //return RedirectToAction("Rezerwacje", "User");
                }
            }
            DBClass.closeConnection();
            ViewBag.Error = "Błąd !!!";
            return View(rezerwacja);
        }

        public IActionResult ChooseTherapist()
        {
            //DataSet ds = new DataSet();

            //DateTime today = DateTime.Today;
            //DateTime dateReservation = today.AddDays(shiftDay - 10);
            //String DataVisitStart = dateReservation.ToString();


            DBClass.openConnection();
            //) CAST(DateOfBirth AS date), convert(Date, DateOfBirth, 23) 
            //DBClass.sql = "select DataVisitStart from reservations where DataVisitStart = '" + dateReservation + "' order by  DataVisitStart";
            DBClass.sql = "select firstNameTherapist, lastNameTherapist, NoteTherapist from therapists";
            DBClass.cmd.CommandType = CommandType.Text;
            DBClass.cmd.CommandText = DBClass.sql;

            DBClass.da = new SqlDataAdapter(DBClass.cmd);
            DBClass.dt = new DataTable();
            DBClass.da.Fill(DBClass.dt);

            // wyciągamy dane
            var listofTherapist = new List<Therapist>();
            int j = 0, i = 0;
            using (SqlDataReader reader = DBClass.cmd.ExecuteReader())
            {
                Debug.WriteLine("Reader długość 01- " + reader.FieldCount);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        for (j = 0; j <= reader.FieldCount - 1; j++) // Looping throw colums
                        {
                            data[j] = reader.GetValue(j).ToString(); 
                        }
                        listofTherapist.Add(new Therapist { firstname = data[0], lastname = data[1], noteTherapist = data[2]});
                    }
                }
            }
            DBClass.closeConnection();

            return View(listofTherapist.ToAsyncEnumerable());
        }

        public IActionResult ChooseData(string lastName)
        {
            var reservation = new ReservationInfo();
            reservation.lastNameTherapist = lastName;
            return View(reservation);
        }

        [HttpPost]
        public IActionResult FinishReservation(OstatecznaRezerwacja rezerwacja)
        {
            //backend - > zapisac dane do bazy danych - rezerwację
            //widok w widoku wypluc jako html pożegnanie
            string[] DaneRezerwacji = new string[99];
            DaneRezerwacji[0] = rezerwacja.lastnameTherapis;
            DaneRezerwacji[1] = rezerwacja.dateReservation;
            DaneRezerwacji[2] = rezerwacja.timeReservation + ":00";
            DaneRezerwacji[3] = loginUserForm;
            DaneRezerwacji[4] = userFirstname;
            DaneRezerwacji[5] = lastUserName;
            Int32.TryParse(rezerwacja.timeReservation, out int tempHour);
            tempHour++;
            DaneRezerwacji[6] = tempHour.ToString() + ":00";

            ViewBag.DaneRezerwacji = DaneRezerwacji;
            DBClass.openConnection();
            //) CAST(DateOfBirth AS date), convert(Date, DateOfBirth, 23) 
            DBClass.sql = "        insert into reservations(DataVisitStart, TimeStartVisit, TimeVisitEnd, IDTherapist, IDUser) " +
                "values('" + DaneRezerwacji[1] + "', '" + DaneRezerwacji[2] + "', '" + DaneRezerwacji[6] + "', " +
                "(select distinct IDTherapist from therapists where therapists.LastNameTherapist = '" + DaneRezerwacji[0] + "'), " +
                "(select distinct users.IDUsers from users where users.nameUser = '" + DaneRezerwacji[3] + "'))";
            //DBClass.sql = "select FirstName, lastName from patients";
            DBClass.cmd.CommandType = CommandType.Text;
            DBClass.cmd.CommandText = DBClass.sql;

            DBClass.da = new SqlDataAdapter(DBClass.cmd);
            DBClass.dt = new DataTable();
            DBClass.da.Fill(DBClass.dt);

            DBClass.closeConnection();
            return View();
        }
    }   
}

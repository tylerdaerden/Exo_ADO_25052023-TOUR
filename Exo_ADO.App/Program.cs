using Exo_ADO.App.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection.Metadata;

#region Définition de la connexion vers la DB
/* ConnectionString Centre
string connectionString = @"Data Source=Forma300\TFTIC;Initial Catalog=Exo_ADO;User ID=Chris;Password=Test1234=;TrustServerCertificate=true;";
*/
/* ConnectionString Steve
 * string connectionString = @"Data Source=STEVEBSTORM\MSSQLSERVER01;Initial Catalog=Exo_ADO_DB;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
 */
/* ConnectionString LocalDB */
string connectionString = @"Data Source=TOURPCDANY\DATAVIZ;Initial Catalog=Exo_ADO.DB;User ID=Chris;Password=Test1234=;Integrated Security=True;TrustServerCertificate=true;";
 

#endregion
using (SqlConnection connection = new SqlConnection())
{
    connection.ConnectionString = connectionString;

    //Afficher l’« ID », le « Nom », le « Prenom » de chaque étudiant depuis la vue « V_Student » en utilisant la méthode connectée
    Console.WriteLine("  Mode connectée");
    Console.WriteLine("  **************");

    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT * FROM [V_Student]";
        command.CommandType = CommandType.Text;

        connection.Open();
        using(SqlDataReader reader = command.ExecuteReader())
        {
            // Lecture de la DB
            while(reader.Read())
            {
                Student student = new Student
                {
                    Id = (int)reader["Id"],
                    LastName = (string)reader["LastName"],
                    FirstName = (string)reader["FirstName"],
                    YearResult = (int)reader["YearResult"],
                    BirthDate = (DateTime)reader["BirthDate"]
                };

                Console.WriteLine($"{student.Id} : {student.FirstName} {student.LastName}");
            }
        }
        connection.Close();
    }
    Console.WriteLine();


    // Afficher l’« ID », le « Nom » de chaque section en utilisant la méthode déconnectée
    Console.WriteLine("  Mode déconnectée");
    Console.WriteLine("  ****************");

    using(SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT * FROM [V_Student]";
        command.CommandType = CommandType.Text;

        // Créer l'adapter avec la config pour la récuperation de donnée
        SqlDataAdapter adapter = new SqlDataAdapter();
        adapter.SelectCommand = command;

        // Utilisation de l'adapter pour populer un "DataSet" / "DataTable"
        DataTable table = new DataTable();
        adapter.Fill(table);
        // Remarque -> Il n'est pas necessaire d'ouvrir et fermer la connexion :)

        // Parcours des resultats
        foreach(DataRow row in table.Rows)
        {
            Console.WriteLine($"{row["Id"]} : {row["LastName"]}");
        }
    }
    Console.WriteLine();

    // Afficher la moyenne annuelle des étudiants
    Console.WriteLine("  Moyenne annuelle des étudiants");
    Console.WriteLine("  ******************************");

    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT AVG(CONVERT(FLOAT, [YearResult])) FROM [V_Student]";
        command.CommandType = CommandType.Text;

        connection.Open();
        double moyenne = (double)command.ExecuteScalar();
        connection.Close();

        Console.WriteLine($"La moyenne est de {moyenne} !");
    }

    //Inserer un nouveau student
    //Instancier un student
    Student moi = new Student
    {
        FirstName = "Steve",
        LastName = "Lorent",
        BirthDate = new DateTime(2000,01,01),
        YearResult = 20,
        SectionID = 1010
    };

    //Insertion en DB
    using (SqlCommand command = connection.CreateCommand())
    {
        string query = "INSERT INTO student (FirstName, LastName, BirthDate, YearResult, SectionID) " +
            " OUTPUT inserted.Id " +
            "VALUES(@prenom, '" + moi.LastName + "', '" +
            moi.BirthDate + "', '" + moi.YearResult + "', '" + moi.SectionID + "')";

        // command.Parameters.Add
        /*SqlParameter PPrenom = new SqlParameter()
        {
            ParameterName = "prenom",
            Value = moi.FirstName,
            Direction = ParameterDirection.Input
        };

        command.Parameters.Add(PPrenom);*/

        command.Parameters.AddWithValue("prenom", moi.FirstName);

        command.CommandText = query;
        connection.Open();
        moi.Id = (int)command.ExecuteScalar();
        connection.Close();
        Console.WriteLine("Nouvel Id : " + moi.Id);
    }

    //Utilisation Requête parametrée

    Student voisin = new Student
    {
        FirstName = "Arthur",
        LastName = "Pendragon",
        BirthDate = new DateTime(2000, 01, 01),
        YearResult = 19,
        SectionID = 1010
    };

    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "INSERT INTO student (FirstName, LastName, BirthDate, YearResult, SectionID) " +
            "OUTPUT [inserted].[Id] "+
            "VALUES (@FirstName, @LastName, @BirthDate, @YearResult, @SectionId)";

        command.Parameters.AddWithValue("FirstName", voisin.FirstName);
        command.Parameters.AddWithValue("LastName", voisin.LastName);
        command.Parameters.AddWithValue("BirthDate", voisin.BirthDate);
        command.Parameters.AddWithValue("YearResult", voisin.YearResult);
        command.Parameters.AddWithValue("SectionId", voisin.SectionID);

        connection.Open();

        voisin.Id = (int)command.ExecuteScalar();
        connection.Close();
    }

    /*using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "DELETE FROM student WHERE Id = 27";
        connection.Open();
        try
        {
            command.ExecuteNonQuery();
        }
        catch(SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        connection.Close();
    }*/

    moi.SectionID = 1320;

    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SP_Student_Update";
        command.CommandType = CommandType.StoredProcedure;
        SqlParameter sp_id = new SqlParameter("id", moi.Id);
        command.Parameters.Add(sp_id);
        SqlParameter sp_yr = new SqlParameter("year_result", moi.YearResult);
        command.Parameters.Add(sp_yr);
        SqlParameter sp_section = new SqlParameter("section_id", moi.SectionID);
        command.Parameters.Add(sp_section);

        connection.Open();
        int update_row = command.ExecuteNonQuery();

        connection.Close();

        if (update_row > 0) Console.WriteLine("Mis à jour!");
    }

    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "SP_Student_Delete";
        command.Parameters.AddWithValue("id",voisin.Id);

        connection.Open() ;
        int delete_row = command.ExecuteNonQuery();
        connection.Close();

        if (delete_row > 0) Console.WriteLine("Suppression effecutée");
    }

    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT * FROM [V_Student]";
        connection.Open();
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Id"]} : {reader["FirstName"]} {reader["LastName"]}");
            }
        }
        connection.Close() ;
    }


    #region Exercice Récap

    //Analysez les besoins et créez une DLL(Librairie de classe) proposant deux classes:
    //« Command » : représentant toute commande que vous souhaitez exécuter, celle ci doit être capable de gérer les paramètres et de permettre l’utilisation des procédures stockées
    //« Connection » : représentant une connexion vers SQL Server celle ci devra implémenter les méthodes « ExecuteScalar », « ExecuteReader », « ExecuteNonQuery » et « GetDataTable ». Chacune d’elle devra recevoir au moins un paramètre de type « Command ».

    //Attention, la fonction « ExecuteReader » devra quant à elle retourner une valeur de type « IEnumerable < ».
    //Refaites tous les exercices, depuis la récupération de données, en utilisant vos nouvelles classes.



    #endregion
}
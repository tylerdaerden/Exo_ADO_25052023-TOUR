namespace ClassLibrary


{
    using Microsoft.Data.SqlClient;
    public class Command : IStored_Proc
    {
        //« Command » : représentant toute commande que vous souhaitez exécuter, celle ci doit être capable de gérer les paramètres et de permettre ,l’utilisation des procédures stockées

        #region Props

        public string ?NomCommande { get; set; }
        public string ?Parametres { get; set; }

        #endregion





        #region Procédures Stockées





        #endregion


    }
}
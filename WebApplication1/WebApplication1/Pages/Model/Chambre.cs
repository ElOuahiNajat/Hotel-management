namespace WebApplication1.Models
{
    public class Chambre
    {
        public int NumeroChambre { get; set; } // Numéro de la chambre (int)
        public bool Disponibilite { get; set; } // Disponibilité de la chambre (bit)
        public string TypeChambre { get; set; } // Type de chambre (string)
        public float Prix { get; set; } // Prix de la chambre (float)
    }
}
namespace la_mia_pizzeria_model.Models
{
    public class Pizzeria
    {
        public string Nome;
        public  List<Pizza> listaPizze;
        public Pizzeria(string nome)
        {
            this.Nome = nome;
            listaPizze = new List<Pizza>();
            this.addPizza(new Pizza(0, "Capricciosa", "Pizza molto buona, per tutti con funghi e altro", "capricciosa.jpg", 10.50));
            this.addPizza(new Pizza(1, "4 formaggi", "Pizza molto buona, per tutti con 4 formaggi", "4form.webp", 7.80));
            this.addPizza(new Pizza(2, "Diavola", "Pizza molto buona, piccante", "d1.jpg", 15.00));
            this.addPizza(new Pizza(3, "Margherita", "La più semplice tra le pizze", "margh.jpg", 13.05));
        }
        public void addPizza(Pizza pizza)
        {
            this.listaPizze.Add(pizza);
        }

        public void delPizza(int? id)
        {
            Pizza? pizzaDaCancellare = listaPizze.Find(item => item.Id == id);
            if(pizzaDaCancellare != null)
            {
                this.listaPizze.Remove(pizzaDaCancellare);
            }
        }
    }
}

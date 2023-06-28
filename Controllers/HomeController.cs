using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WaffleWebWorksTpFinal.Models;
// usibg
using Microsoft.Data.SqlClient;
using System.Security;
using System;

namespace WaffleWebWorksTpFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MovieADO;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        // guardo lo necesario para conectar a la base de datos en un string para despues pegar el string y listo
        List<Carrera> list = new List<Carrera>();
        List<CarreraIma> listima = new List<CarreraIma>();
        //lisrta para guardar los resultados del select y mostrar en el sitio web
        public IActionResult Index()
        {
            try
            {
                //prepara la conectar
                SqlConnection connection = new SqlConnection(connectionString);
                //abre la coencta
                connection.Open();
                // guarda en un string el codigo sql a ejecutar
                string queryString = "Select * from CarreraImagen";
                // creoq ue guarda en un variable(command) la consulta sql lo que debe ejecutar y en que conexion para saber en donde se debe ejecutar
                SqlCommand command = new SqlCommand(queryString, connection);
                //ejecuta el codigo sql y guarda en reader los resultados
                SqlDataReader reader = command.ExecuteReader();
                //repite las filas obtenidas
                while (reader.Read())
                {
                    // guarda en la variable los iten por categoria o columnas
                    CarreraIma movieADOs = new CarreraIma()
                    {
                        //guarda por elemento del modelo carrera
                        Id = int.Parse(reader[0].ToString()),
                        Name = reader[1].ToString(),      
                        Description = reader[2].ToString(),
                        Imagen =  reader[3].ToString(),
                };
                    //guarda en la lista todo lo guardado en la variable anterioir
                    listima.Add(movieADOs);

                }
                //cierrra la conexion
                connection.Close();
                //muestra la lista de resutltado
                return View(listima);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CarreraIma mov)
        {
            try
            {

                SqlConnection connection = new SqlConnection(connectionString);

                connection.Open();

                string queryString = "INSERT INTO CarreraImagen (carrera, Descripcion, nombreimagen) VALUES ( @Carrera, @Description, @nombreimagen);";
               
                SqlCommand command = new SqlCommand(queryString, connection);
                //  command.ExecuteReader(queryString);
                //toma los parametros obtenidos para despues agregarlos en el consulta sql
                command.Parameters.AddWithValue("@Carrera", mov.Name);
                command.Parameters.AddWithValue("@Description", mov.Description);      
                command.Parameters.AddWithValue("@nombreimagen", mov.Imagen);
                //ejecuta la consulta
               // SqlDataReader reader = command.ExecuteReader();
                command.ExecuteNonQuery();
                //SqkData render = command.ExecuteReader();

                connection.Close();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Detalle(int id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                //filtra por id
                string queryString = "Select * from CarreraImagen Where Id=@ID";
                //string queryString = "INSERT INTO carrera (Id, titulo, fecha, genero, precio) VALUES (10, 'Delta', 15/12/1999, 'magia', 600);";
                SqlCommand command = new SqlCommand(queryString, connection);
                //  command.ExecuteReader(queryString);
                //necesario hacer este para que sepa a que id filtrar
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    CarreraIma movieADOs = new CarreraIma()
                    {
                        Id = int.Parse(reader[0].ToString()),
                        Name = reader[1].ToString(),
                        //
                        //ReleaseDate = DateTime.Parse(reader[2].ToString()),
                        Description = reader[2].ToString(),
                        Imagen = reader[3].ToString(),
                    };
                    return View(movieADOs);

                    //  list.Add(movieADOs);
                    //list.Add(movieADOs1);
                }



                connection.Close();
                return RedirectToAction("Index");

            }
            catch (Exception)
            {

                throw;
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        //esto seria editor pero el editor sin httpost es lo mismo que el de arriba. ademas un poco de variedad
        public async Task<IActionResult> Detalle(int id, CarreraIma mov)
        {

            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string queryString = "update CarreraImagen   set  carrera=@Name, Descripcion=@Description, nombreimagen=@nombreimagen  where Id = @id";
                //string queryString = "INSERT INTO MovieADO (Id, titulo, fecha, genero, precio) VALUES (10, 'Delta', 15/12/1999, 'magia', 600);";
                SqlCommand command = new SqlCommand(queryString, connection);
                //  command.ExecuteReader(queryString);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Name", mov.Name);
                command.Parameters.AddWithValue("@Description", mov.Description);

                //nameimagen = nameimagen.Replace("/view?usp=sharing/","");

                command.Parameters.AddWithValue("@nombreimagen", mov.Imagen);




                //SqlDataReader reader = command.ExecuteReader();
                command.ExecuteNonQuery();


                connection.Close();

                return RedirectToAction("Detalle");

            }
            catch (Exception)
            {

                throw;
            }


        }
        // intente hacer lo mismo que arriba pero no pude, la razon nose 
        public IActionResult Delete(int id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string queryString = "Select * from CarreraImagen Where Id=@ID";
                //string queryString = "INSERT INTO carrera (Id, titulo, fecha, genero, precio) VALUES (10, 'Delta', 15/12/1999, 'magia', 600);";
                SqlCommand command = new SqlCommand(queryString, connection);
                //  command.ExecuteReader(queryString);
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    CarreraIma movieADOs = new CarreraIma()
                    {
                        Id = int.Parse(reader[0].ToString()),
                        Name = reader[1].ToString(),
                        //
                        //ReleaseDate = DateTime.Parse(reader[2].ToString()),
                        Description = reader[2].ToString(),
                        Imagen = reader[3].ToString()
                        
                    };
                    return View(movieADOs);

                    //  list.Add(movieADOs);
                    //list.Add(movieADOs1);
                }



                connection.Close();
                return RedirectToAction("Index");

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete( CarreraIma mov)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string queryString = "delete from CarreraImagen where Id = @Id";
                //string queryString = "INSERT INTO MovieADO (Id, titulo, fecha, genero, precio) VALUES (10, 'Delta', 15/12/1999, 'magia', 600);";
                SqlCommand command = new SqlCommand(queryString, connection);
                //  command.ExecuteReader(queryString);
                command.Parameters.AddWithValue("@Id", mov.Id);


               // SqlDataReader reader = command.ExecuteReader();
                command.ExecuteNonQuery();


                connection.Close();

                return RedirectToAction("Index");

            }
            catch (Exception)
            {

                throw;
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


/*
 CREATE TABLE [dbo].[CarreraImagen] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [carrera]      NVARCHAR (100) NOT NULL,
    [Descripcion]  VARCHAR (MAX)  NOT NULL,
    [nombreimagen] VARCHAR (MAX)  NOT NULL
);

https://localhost:7021/images/logo3.jpeg

 */
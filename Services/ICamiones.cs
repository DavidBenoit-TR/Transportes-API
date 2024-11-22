using DTO;
using System.Data.Entity.Validation;
using Transportes_API.Models;

namespace Transportes_API.Services
{
    public interface ICamiones
    {
        //es una estructura que define un contrato o conjunto de métodos y
        //propiedades que una clase debe implementar.
        //Una interfaz establece un conjunto de requisitos que cualquier clase
        //que la implemente debe seguir. Estos requisitos son declarados en la
        //interfaz en forma de firmas de métodos y propiedades,
        //pero la interfaz en sí misma no proporciona ninguna implementación
        //de estos métodos o propiedades.Es responsabilidad de las clases que
        //implementan la interfaz proporcionar las implementaciones concretas de
        //estos miembros.

        //Las interfaces son útiles para lograr la abstracción y la reutilización
        //de código en C#.

        //GET
        List<Camiones_DTO> getCamiones();

        //GETbyID
        Camiones_DTO getCamionbyID(int id);

        //Insert
        string insertCamion(Camiones_DTO camion);

        //Update
        string updateCamion(Camiones_DTO camion);
        //Delete
        string deleteCamion(int id);
    }

    //la clase que implementa la interfaz y declare la implementación de la lógica de los métodos existentes
    public class CamionesService : ICamiones
    {
        //variable para crear el contexto
        private readonly TransportesContext _context;

        //constructor para inicializar el contexto
        public CamionesService(TransportesContext context)
        {
            _context = context;
        }

        //Impementación de métodos
        public string deleteCamion(int id)
        {
            try
            {
                //buscar si existe en la D unelemento que coincida con id que me mandan (recupero el objeto)
                Camiones _camion = _context.Camiones.Find(id);
                //valido que realmente recuperé mi objeto
                if (_camion == null)
                {
                    return $"No se encontró el objeto con identificador {id}";
                }

                try
                {
                    //remuevo el objeto del contexto
                    _context.Camiones.Remove(_camion);
                    //impacto la BD 
                    _context.SaveChanges();
                    //respondo
                    return $"Camión {id} eliminado con éxito";
                }
                catch (DbEntityValidationException ex)
                {
                    string resp = "";
                    //recorro todos los posibles errores de la entidad referencial
                    foreach (var error in ex.EntityValidationErrors)
                    {
                        //recorro los detalles de cada error
                        foreach (var validationError in error.ValidationErrors)
                        {
                            resp = "Error en la Entidad: " + error.Entry.Entity.GetType().Name;
                            resp += validationError.PropertyName;
                            resp += validationError.ErrorMessage;
                        }
                    }
                    return resp;
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public Camiones_DTO getCamionbyID(int id)
        {
            Camiones_DTO respuesta = new Camiones_DTO();
            Camiones _camion = _context.Camiones.Find(id);
            if (_camion != null)
            {
                respuesta.ID_Camion = _camion.ID_Camion;
                respuesta.Matricula = _camion.Matricula;
                respuesta.Capacidad = _camion.Capacidad;
                respuesta.Tipo_Camion = _camion.Tipo_Camion;
                respuesta.UrlFoto = _camion.UrlFoto;
                respuesta.Marca = _camion.Marca;
                respuesta.Modelo = _camion.Modelo;
                respuesta.Kilometraje = _camion.Kilometraje;
                respuesta.Disponibilidad = _camion.Disponibilidad;
            }
            return respuesta;
        }

        public List<Camiones_DTO> getCamiones()
        {
            try
            {
                //lista de cmaiones del original
                List<Camiones> lista_original = _context.Camiones.ToList();
                //lista de DTOS
                List<Camiones_DTO> lista_salida = new List<Camiones_DTO>();
                //recorro cada camión y genereo un nuevo DTO con Automaper
                foreach (var camion in lista_original)
                {
                    //usamos el automapper para
                    Camiones_DTO camionDTO = DynamicMapper.Map<Camiones, Camiones_DTO>(camion);
                    lista_salida.Add(camionDTO);
                }
                return lista_salida;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public string insertCamion(Camiones_DTO camion)
        {
            try
            {
                //creo un camión del modelo original
                Camiones _camion = new Camiones();
                //asignos los valores del objeto DTO del parámetro al objeto del modelo original
                _camion = DynamicMapper.Map<Camiones_DTO, Camiones>(camion);
                try
                {
                    //añadimos el objeto al contexto
                    _context.Camiones.Add(_camion);
                    //impactamos en la BD
                    _context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    string resp = "";
                    //recorro todos los posibles errores de la entidad referencial
                    foreach (var error in ex.EntityValidationErrors)
                    {
                        //recorro los detalles de cada error
                        foreach (var validationError in error.ValidationErrors)
                        {
                            resp = "Error en la Entidad: " + error.Entry.Entity.GetType().Name;
                            resp += validationError.PropertyName;
                            resp += validationError.ErrorMessage;
                        }
                    }
                    return resp;
                }
                return "Camión insertado con éxito";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public string updateCamion(Camiones_DTO camion)
        {
            try
            {
                //creo un camión del modelo original
                Camiones _camion = new Camiones();
                //asignos los valores del objeto DTO del parámetro al objeto del modelo original
                _camion = DynamicMapper.Map<Camiones_DTO, Camiones>(camion);
                try
                {
                    //añadimos el objeto al contexto
                    _context.Entry(_camion).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    //impactamos en la BD
                    _context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    string resp = "";
                    //recorro todos los posibles errores de la entidad referencial
                    foreach (var error in ex.EntityValidationErrors)
                    {
                        //recorro los detalles de cada error
                        foreach (var validationError in error.ValidationErrors)
                        {
                            resp = "Error en la Entidad: " + error.Entry.Entity.GetType().Name;
                            resp += validationError.PropertyName;
                            resp += validationError.ErrorMessage;
                        }
                    }
                    return resp;
                }
                return $"Camión {camion.ID_Camion} Actualizado con éxito";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}

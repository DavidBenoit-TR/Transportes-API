using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Transportes_API.Models;
using Transportes_API.Services;

namespace Transportes_API.Controllers
{
    [Route("api/[controller]")] //se declara el espacio de nombre
    [ApiController]//establece el trato del contrador
    public class CamionesController : ControllerBase
    {
        //vraiables para interfaz y el contexto
        private readonly ICamiones _service;
        private readonly TransportesContext _context;

        //contructor para inicializar mi servicio y mi contexto
        public CamionesController(ICamiones service, TransportesContext context)
        {
            this._context = context;
            this._service = service;
        }

        //GET
        [HttpGet] //declara el método a realizar en la petición
        [Route("getCamiones")]
        public List<Camiones_DTO> getCamiones()
        {
            //creo una lista de objetos DTO y la lleno con mi servicio
            List<Camiones_DTO> lista = _service.getCamiones();
            return lista;//retorno la lista
        }

        //GET by ID
        [HttpGet]
        [Route("getCamion/{id}")] //además de la ruta, establezco un parámetro a llegar desde la ruta
        public Camiones_DTO getCamion(int id)
        {
            Camiones_DTO camion = new Camiones_DTO(); //creo un objeto del modelo original
            camion = _service.getCamionbyID(id); //lleno este objeto desde la lista
            return camion; //retorno el objeto
        }

        //POST (Insertar)
        [HttpPost]
        [Route("insertCamion")]
        //los métodos IActionResult retornan una respuesta a las API en un formato preestablecido
        //capaz de ser leido por cualquier cliente http
        //por otro lado FromBody determina que el parámetro que se espera será tomado del propio cuerpo de la peticón POST
        public IActionResult insertCamion([FromBody] Camiones_DTO camion)
        {
            string respuesta = _service.insertCamion(camion);
            return Ok(new { respuesta });//su retorno es un nuevo objeto de tipo OK
                                         //Siendo OK la respuesta a la petción HTTP
        }

        //PUT(actualizar/modifcar)
        [HttpPut]
        [Route("updateCamion")]
        public IActionResult updateCamion([FromBody] Camiones_DTO camiones)
        {
            string respuesta = _service.updateCamion(camiones);
            return Ok(new { respuesta });
        }

        //DELETE (Eliminar)
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult deleteCamion(int id)
        {
            string respuesta = _service.deleteCamion(id);
            return Ok(new { respuesta });
        }
    }
}

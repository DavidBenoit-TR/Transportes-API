using System;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Transportes_API.Services
{
    public class DynamicMapper
    {
        //método que mapea de forma dinámica tipos de objetos (por ejemplo modelos originales a DTO y viceversa)
        public static TDestination Map<TSource, TDestination>(TSource source)
            where TSource : class //se declara una clase abstracta como tipo de datos de entrada
            where TDestination : class, new() //se declara una clase abstracta como tipo de datos de salida
        {
            //valido si existe y contiene información la clase de origen
            if (source == null) throw new ArgumentNullException("source");

            var destination = new TDestination(); //creo una instancia del objeto de salida

            //recuperar las propiedades (los atributos de mis elementos) usando la biblioteca system.reflexion
            //Mediante reflexión, puedes acceder a las propiedades de un tipo (clase, estructura, etc.) en tiempo de ejecución, incluso si no conoces el tipo exacto en tiempo de compilación.
            //GetProperties: Devuelve un array con todas las propiedades públicas del tipo especificado.
            //BindingFlags: Opciones que especifican qué miembros buscar(públicos, privados, estáticos, etc.).
            //using System.Reflection;
            var sourceProperties = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var destinationProperties = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //recorro todos los atributos y propiedades del objeto de origen para equipararlos con el objeto de salida
            foreach (var sourceProperty in sourceProperties)
            {
                //recupero cada propiedad de la clase donde empate tanto el nombre de la propiedad como el tipo de dato (aquí se mapean los objetos)
                var destinationProperty = destinationProperties.FirstOrDefault(dp => dp.Name.ToLower() == sourceProperty.Name.ToLower() && dp.PropertyType == sourceProperty.PropertyType);

                //si la propiedad es accesible y tiene un valor, paso el dato del origen al destino
                if (destinationProperty != null && destinationProperty.CanWrite)
                {
                    //GetValue: Lee el valor actual de la propiedad para un objeto.
                    //SetValue: Establece un nuevo valor para la propiedad de un objeto.
                    var value = sourceProperty.GetValue(source);
                    destinationProperty.SetValue(destination, value);
                }
            }
            //retorno el nuevo tipo de dato
            return destination;
        }
    }
}

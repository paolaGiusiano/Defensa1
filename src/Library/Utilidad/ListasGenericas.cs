using System.Collections.Generic;
namespace CoreBot
{   
    /// <summary>
    /// Clase de utilidad que contiene listas genericas
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ListaGen<T> where T : ICampoUnico
    {
        /// <summary>
        /// Metodo que agrega un elemento a la lista
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="elem"></param>
        /// <param name="unico"></param>
        /// <returns></returns>
        public static List<T> AgregarElemento(List<T> lista, T elem, bool unico = false)
        {

            if(lista == null)
            {
                lista = new List<T>();
            }
            List<T> result = new List<T>();
            if(lista.Count != 0)
            {
                if (unico == false)
                {
                    result.Add(elem);
                }
                else
                {
                    int contadorUnico = 0;
                    for (int i = 0; i < lista.Count; i++)
                    {
                        if(lista[i].GetCampoUnico() == elem.GetCampoUnico())
                        {
                            contadorUnico++;
                        }

                    }
                    if(contadorUnico == 0)
                    {
                        result = lista;
                        result.Add(elem);
                    }
                    else
                    {
                        result = lista;
                    }

                }
            }


            return result;  
        }
        /// <summary>
        /// Metodo que modifica un elemento de la lista
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="elem"></param>
        /// <returns></returns>
        public static List<T> ModificarElemento(List<T> lista, T elem)
        {

            if (lista == null)
            {
                lista = new List<T>();
            }
            List<T> result = new List<T>();
            if (lista.Count != 0)
            {
                for (int i = 0; i < lista.Count; i++)
                {
                    if (lista[i].GetCampoUnico() != elem.GetCampoUnico())
                    {
                        result.Add(lista[i]);
                    }
                    else
                    {
                        result.Add(elem);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Metodo que borra un elemento de la lista
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="elem"></param>
        /// <returns></returns>
        public static List<T> BorrarElemento(List<T> lista, T elem)
        {

            if (lista == null)
            {
                lista = new List<T>();
            }
            List<T> result = new List<T>();
            if (lista.Count != 0)
            {
                for (int i = 0; i < lista.Count; i++)
                {
                    if (lista[i].GetCampoUnico() != elem.GetCampoUnico())
                    {
                        result.Add(lista[i]);
                    }
                }
            }
            return result;
        }
    }
}
namespace CoreBot
{
    /// <summary>
    /// Una interfaz que define una abstracción para un calculador de distancias genérico.
    /// </summary>
    /// <remarks>
    /// Esta interfaz fue creada siguiendo el principio de inversión de dependencias para evitar que los comandos
    /// concretos dependan de calculadores de distancias concretos; en su lugar los comandos concretos dependen de esta
    /// abstracción.
    /// Entre otras cosas est permite cambiar el calculador de distancias en tiempo de ejecución, para utilizar uno en
    /// los casos de prueba que retorna resultados conocidos para direcciones conocidas, y otro en la versión final para
    /// buscar usando una API de localizaciones.
    /// </remarks>
    public interface IDistanceCalculator
    {
        /// <summary>
        /// Determina si existe una dirección.
        /// </summary>
        /// <param name="fromAddress">Una de las direcciones a buscar.</param>
        /// <param name="toAddress">Una de las direcciones a buscar.</param>
        /// <returns>true si la dirección existe; false en caso contrario.</returns>
        IDistanceResult CalculateDistance(string fromAddress, string toAddress);
    }

    /// <summary>
    /// Una interfaz que define una abstracción para el resultado de calcular distancias.
    /// </summary>
    public interface IDistanceResult
    {
        /// <summary>
        /// Obtiene un valor que indica si la dirección de origen para el cálculo de distancias existe; sólo se puede
        /// calcular la distancia entre direcciones que existen.
        /// </summary>
        bool FromExists { get; }

        /// <summary>
        /// Obtiene un valor que indica si la dirección de destino para el cálculo de distancias existe; sólo se puede
        /// calcular la distancia entre direcciones que existen.
        /// </summary>
        bool ToExists { get; }

        /// <summary>
        /// La distancia calculada.
        /// </summary>
        double Distance { get; }

        /// <summary>
        /// El tiempo en llegar del origen al destino.
        /// </summary>
        double Time { get; }
    }
}
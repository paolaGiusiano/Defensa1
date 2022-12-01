# Integrantes
Santiago Avelino,
Andrés Arancio,
Ignacio Perez,
Ivan Lomando,
Paola Giusiano

# Objetivo del Programa
En alineamiento con los [objetivos de desarrollo sostenible de las Naciones Unidas](https://www.un.org/sustainabledevelopment/es/objetivos-de-desarrollo-sostenible/), en particular con el [Objetivo 8: Trabajo decente y crecimiento economico](https://www.un.org/sustainabledevelopment/es/economic-growth/), se plantea la siguiente pregunta:
> Como podemos nosotros, estudiantes del curso de Programacion II, ayudar a personas a encontrar trabajo, de tal forma que el trabajo sea accesible, decente, y apunte al desarrollo y crecimiento de pymes y trabajadores independientes?
La propuesta resultante es la creacion de un chatbot conversacional que, en base de las necesidades de un prospecto empleador, responda con quienes otorgan ese servicio, ayudando a conectar quienes necesitan un servicio y quienes los ofrecen. Adicionalmente, se apunta a que el chatbot habilite la habilidad de calificar tanto el trabajador como el empleador de una oferta de servicio, asi todo usuario del chatbot puede estar al tanto de la reputacion de cada uno, fomentando un ambiente de trabajo respetuoso, efectivo y decente.

# Diseño a Alto Nivel
El espiritu detras del diseño y arquitectura de este programa se basa en la modularidad de sus clases y metodos. Debido a la naturaleza cambiante del programa con cada instancia de feedback recibido por entrega, se intenta que el codigo cumpla con tanta diligencia como sea posible con los principios de GRASP y SOLID, en particular haciendo enfasis en Expert, SRP, OCP y DIP, con el objetivo de permitir al programa la flexibilidad de ser expandido a futuro sin alterar su funcionamiento ya existente con el minimo esfuerzo y alteracion al diseño y arquitectura posible.

## Acercamiento a Arquitectura
Se apunta a crear una division clara en tres capas del programa: su Dominio, su Motor y sus Handlers. El Dominio actua como capa de mas alto nivel, reduciendo el acoplamiento y interdependencia de las mismas a un minimo, con el Motor siendo dependientes de las clases de dominio y actuando como punto de interaccion con los Handlers, y los Handlers siendo el punto de interaccion directo del usuario.
Debido a la incremental complejidad del programa, se propone separar el Motor a su vez en dos subcapas: Gestores y Buscadores. Los Gestores actuan como subcapa de mas alto nivel en Motor, solamente interactuando con el Dominio. Los Buscadores no interactuan con el Dominio de no ser necesario, accediendolo a travez de los Gestores.
Se observa que con este acercamiento el acoplamiento y la interdependencia entre las clases incrementa con forme uno baja de nivel en su arquitectura, pero a su vez se retiene la capacidad de cada metodo de ser delegado facilmente por otro, hipotetico metodo en una capa inferior, cumpliendo con el objetivo del diseño.

## Patrones de Diseño Aplicados
Se utilizan cinco patrones de diseño principales para el funcionamiento del motor del bot. Los gestores y buscadores utilizan el patron de Singleton y Facade, Contratos y Servicios aplican Observer, mientras que los handlers implementan Chain of Responsability y State.

Se aplica el patron [Singleton](https://refactoring.guru/es/design-patterns/singleton) manteniendo la existencia de una unica instancia de cada Gestor y Buscador del programa. Esto es necesario ya que, a falta de una base datos que acceder, el tener una lista unica para Categorias, Contratos, Servicios y Usuarios es imperativo para el poder salvar datos del programa. De forma similar, se requiere un buscador unico para Servicios y Usuarios para asi poder evitar conflictos de tener multiples busquedas a un Gestor actuando en simultaneo.

Se aplica el patron de [Facade](https://refactoring.guru/es/design-patterns/facade) con los buscadores y gestores, ya que estos agrupan funcionalidades complejas que requieren llamar multiples clases para cumplirse, permitiendo tambien aislar las funcionalidades de las clases de dominio de los handlers a traves de las clases de motor.

Se aplica el patron [Observer](https://refactoring.guru/es/design-patterns/observer) para notificar a los usuarios de la plataforma cuando ocurre un evento dramatico a los Servicios o Contratos a los que estan suscritos, en particular cuando a un Servicio se le da de baja y cuando un Contrato es evaluado exitosamente.

Se aplica Chain of [Chain of Responsibility](https://refactoring.guru/design-patterns/chain-of-responsibility) en los handlers del sistema, permitiendo la creacion de una secuencia de operadores que se encargan de comandos especificos. De ningun eslabon de la cadena de responsabilidad puede procesar el comando, se devuelve automaticamente una excepcion.

Se aplica [State](https://refactoring.guru/es/design-patterns/state) en los handlers del sistema, permitiendo que cada handler pueda manejar cada estado del comando del que esta a cargo, reduciendo la necesidad de extra handlers innecesarios.

## Acercamiento a Testing
Se separan los casos de prueba en dos grupos distintos, pruebas individuales de cada metodo del programa y una clase de testing que encapsule el funcionamiento real de cada User Story propuesta. Las pruebas individuales se realizaron en conjunto con la creacion de cada clase, en un acercamiento de "Testing First", mientras que las pruebas de las User Stories se realizaron a completitud del Motor del programa.

Debido al uso de Singletons para los Gestores y Buscadores, se entiende la posiblidad de efectos colaterales con cada caso de prueba al realizar una prueba global. Se confirma que las pruebas realizadas en la clase "TestUserStories" se hicieron una a la vez, por lo que los efectos colaterales se identificaron al final de la implementacion de las segunda entrega. Como forma de mitigar este riesgo, se tomaron las siguientes dos acciones: No reutilizar IDs para los datos de prueba; y utilizar One Time Setups en lugar de Setups cada vez que sea posible.


# Implementacion de Patrones GRASP y SOLID
A continuacion se describen momentos notables de la aplicacion de cada patron GRASP & SOLID durante el proyecto.
## Expert
Si bien se apunta a aplicar Expert en todo el programa, se resalta su uso en los Gestores de Categoria, Contrato, Servicio y Usuario. Cada uno de estos posee toda la informacion relacionada a cada uno de sus respectivas clases de dominio, incluyendo la lista que contiene todas las instancias de los mismos en el sistema. Por lo tanto, cada uno de estos Gestores es responsable exclusivamente del mantenimiento de estas listas, incluyendo el crear, remover y modificar las instancias de sus repectivas clases de dominio.

## SRP
Es notable el uso de SRP en las clases de BuscadorServicios y BuscadorUsuario. Si bien, en teoria, la funcionalidad de estas clases era posible cumplirla con sus respectivos Gestores, para aplicar SRP consideramos necesario que exista una clase que se dedique explicitamente a las diferentes opciones de busqueda en las listas de Servicio y Usuario. Resaltamos la diferencia entre el caso de estas clases y los Gestores de Contrato y Categoria, que si bien tambien requieren poder buscar instancias de sus clases de dominio, no poseen ningun requerimiento que precise busquedas mas complejas, por lo que se considero mas relevante aplicar Expert y dejarlas sin clases de busqueda dedicadas.

## Polimorfismos
Para la aplicacion de polimorfismos se resalta la arquitectura de clases de usuario (Persona, Usuario, Administrador, Empleador y Trabajador). Si bien hubiera sido posible simplemente tener una clase por cada perfil, debido a los campos y responsabilidades en comun entre Empleador y Trabajador, se creo Usuario como su superclase; y con la similitud entre Usuario y Administrador, se creo la superclase Persona. El programa principalmente funciona en base de Persona o Usuario, dependiendo si es posible para el Administrador acceder a alguna funcionalidad o no, y solamente hace uso de Administrador, Empleador y Trabajador de ser necesarios los datos exclusivos que tiene cada uno. Esto creo la necesidad de tener clases como Autenticar, para asegurar el funcionamiento del programa cumpla con los requerimientos de las User Stories.

## LSP
De forma similar, se tomaron las acciones necesarias para que Persona y Usuario cumplieran LSP, al ubicar las validaciones pertinentes para que cuando el programa llamara a una Persona o Usuario, el comportamiento de estos fuera igual para cualquier subclase de los mismos. Es necesario mencionar, sin embargo, los escenarios en los que este patron no se cumple, especificamente cuando es necesario validar el subtipo de una Persona para permitir o no el acceso a ciertas funcionalidades, a traves de la clase Autenticar.

## Creator
Se recalca como uso notable de Creator a los Gestores de Categoria, Contrato, Servicio y Usuario, ya que son estos quienes crean las instancias de sus respectivas clases de dominio, y, siguiendo el patron, deben de hacerlo ya que sus instancias singleton son las que contienen las listas de las mismas. De forma similar, Persona crea la lista de Notificaciones a ser llenada por el Publisher de cada objeto al que la persona esta subscrita, debido que esta contiene la misma lista.

## OCP
Si bien se intenta aplicar OCP en todo el programa, un caso notable en el que se debieron tomar medidas para cumplirlo es con las calificaciones de Contrato. Si bien en un principio pensamos como opcion el mantener una unica lista de dos Calificaciones, una para Empleador y otra para Trabajador, que fueran populadas por el mismo metodo, se encontro que el separar estos en dos atributos separados, tratados por dos metodos separados reduciria la posibilidad de tener que modificar los mismos a futuro y abriria mas opciones para expandirlos.

## DIP
Se puede observar la aplicacion de DIP en el programa en general con la segmentacion de clases que tenemos. Por definicion, las clases de dominio de alto nivel (Categoria, Calificacion y Notificacion), no poseen ninguna dependencia. Las clases de mas bajo nivel en el dominio (la arquitectura de usuario, de la cual ninguna es una clase abstracta, ya que deben ser instanciadas, el Contrato y Servicio) poseen referencias solamente a las clases de mas alto nivel. Debajo de las clases de dominio encontramos el motor y las clases de utilidad, que referencian solamente a las clases de dominio, y al fondo de la jerarquia se encuentran los handlers, que referencian a las clases sobre las mismas.

## ISP
No hubieron muchas oportunidades que encontramos de aplicar ISP, debido a que la mayoria de las clases no dependen de tipos que no usan por default. La unica instancia que merece ser mencionada es durante la aplicacion del patron Observer, se crean las Notificaciones que son usadas exclusivamente por los Publishers, de tal forma que los publishers no puedan acceder directamente a la informacion de las clases de Persona y sus subclases.

## Low Coupling & High Cohesion
En general, para cumplir con ambos principios, se toma la consideracion de que el acoplamiento entre las clases necesariamente se volvera mayor mientras mas se descienda en la arquitectura del programa. Es decir, clases de mas bajo nivel estaran mas acopladas que las de mas alto nivel. Con esto en mente, decidimos reducir la cantidad de acoplamiento en las clases de mas alto nivel a un minimo necesario, para asegurarnos que el acoplamiento total del sistema sea el mas bajo posible.

De forma similar, se entiende que es necesario una cohesion mas alta de las clases para las clases de mas bajo nivel, ya que estas manejaran las funcionalidades mas complejas del sistema. Con esto en mente, se intenta maximizar la cohesion de las clases de mas bajo nivel (handlers, gestores y buscadores), para incrementar la cohesion total del sistema.

## Law of Demeter
No se encontraron muchas oportunidades de aplicar Law of Demeter, ya que en la mayoria de los casos se vio necesaria la comunicacion entre las clases de dominio y los gestores. De tal forma, entendemos que el principio no trae mucho valor a la aplicacion de nuestro programa.

# Instrucciones de uso del Programa
Una vez activado, el bot se puede llamar desde @pii_2022_equipo_12_bot en Telegram. El mismo responde a comandos establecidos en los handlers con un mensaje, y envia un reporte de los comandos enviados por el usuario en la consola. Es importante notar que el bot posee validaciones para prevenir usuarios de acceder con mas de un perfil, por lo tanto para probar el bot efectivamente se requieren cuatro IDs de Telegram distintas.

Para mas detalles, agregamos a la entrega [cuatro videos](https://correoucuedu-my.sharepoint.com/personal/ivan_lomando_correo_ucu_edu_uy/_layouts/15/onedrive.aspx?id=%2Fpersonal%2Fivan%5Flomando%5Fcorreo%5Fucu%5Fedu%5Fuy%2FDocuments%2Fprogramacion%20%202&ga=1) mostrando el funcionamiento de cada User Story en vivo.
## Saludar
Entrada: "hola", "buenas" o "hello"; Salida: "Hola! Bienvenido al Bot de Trabajo!"
## Registrar un Trabajador
Entrada: "registrar", "registrarme" o "agregar usuario"; Salida: "Por favor defina que tipo de usuario quiere registrar: empleador o trabajador".

Entrada: "trabajador"; Salida: "Siguiente, por favor inserte el nombre del usuario a registrar."

Entrada: Un nombre no vacio; Salida: "Siguiente, por favor inserte el apellido del usuario a registrar."

Entrada: Un apellido no vacio; Salida: "Siguiente, por favor inserte el email del usuario a registrar."

Entrada: Un email no vacio; Salida: "Siguiente, por favor inserte la contraseña del usuario a registrar. Debe ser al menos 8 caracteres."

Entrada: Una password de 8 o mas caracteres; Salida: "Siguiente, por favor inserte la direccion del usuario a registrar en el siguiente formato: Departamento, Calle numero de edificio"

Entrada: Una direccion real; Salida: "Creando usuario. Cuando quiera continuar, digamelo."

Entrada: "Next"; Salida: "Se ha creado el trabajador {nombre} {apellido} con el ID {id de Telegram}"

El bot crea un Trabajador con los parametros establecidos el ID del usuario loggeado en Telegram. Su calificacion inicial es 0.
## Registrar un Empleador
Entrada: "registrar", "registrarme" o "agregar usuario"; Salida: "Por favor defina que tipo de usuario quiere registrar: empleador o trabajador".

Entrada: "empleador"; Salida: "Siguiente, por favor inserte el nombre del usuario a registrar."

Entrada: Un nombre no vacio; Salida: "Siguiente, por favor inserte el apellido del usuario a registrar."

Entrada: Un apellido no vacio; Salida: "Siguiente, por favor inserte el email del usuario a registrar."

Entrada: Un email no vacio; Salida: "Siguiente, por favor inserte la contraseña del usuario a registrar. Debe ser al menos 8 caracteres."

Entrada: Una password de 8 o mas caracteres; Salida: "Siguiente, por favor inserte la direccion del usuario a registrar en el siguiente formato: Departamento, Calle numero de edificio"

Entrada: Una direccion real; Salida: "Creando usuario. Cuando quiera continuar, digamelo."

Entrada: "Next"; Salida: "Se ha creado el empleador {nombre} {apellido} con el ID {id de Telegram}"

El bot crea un Empleador con los parametros establecidos el ID del usuario loggeado en Telegram. Su calificacion inicial es 0 asi no tiene precedencia sobre los Trabajadores con mayor antiguedad en el sistema.
## Agregar un Administrador
Entrada: "agregar administrador"; Salida: "La creacion de administradores esta restringida a otros admins y miembros del equipo de desarrollo. Inserte master password o su password personal."

Entrada: Una password de un administrador existente o "P2Bot"; Salida: "Por favor introduzca el Nombre del Administrador"

Entrada: Un nombre no vacio; Salida: "Siguiente, introduzca el Apellido del Administrador"

Entrada: Un apellido no vacio; Salida: "Siguiente, introduzca el Email del Administrador"

Entrada: Un email no vacio; Salida: "Siguiente, introduzca la password del Administrador. Recuerde que debe tener 8 o mas caracteres."

Entrada: Un password con 8 caracteres o mas; Salida: "Creando Admin. Escriba Next o cualquier mensaje para continuar."

Entrada: "Next"; Salida: "Admin {nombre} {apellido} ha sido creado"

El bot crea un Administrador con los parametros establecidos el ID del usuario loggeado en Telegram. Su calificacion inicial es 0 asi el sistema declara que no tiene informacion acerca de la experiencia de Trabajadores con el mismo.
## Crear Categoria
Limitacion: Solo accesible por Administradores

Entrada: "crear categoria", "crear una categoria", "agregar una categoria"; Salida: "Por favor, elija el nombre de la categoria."

Entrada: Un nombre de una categoria nueva; Salida: "Su categoria {nombre} ha sido creada con exito."

El bot agrega la categoria creada a la lista de categorias del sistema.
## Crear Servicio
Limitacion: Solo accesible por Trabajadores

Entrada: "crear servicio", "crear un servicio" o "agregar servicio"; Salida: "Por favor, inserte la descripcion del servicio."

Entrada: Una descripcion no vacia del servicio; Salida: "Ahora, por favor, elija una categoria para el servicio de la siguiente lista: {lista de todas las categorias en el sistema}"

Entrada: Una de las categorias existentes en el sistema; Salida: "Para continuar, elija la forma de pago de su servicio: Debito; Credito; Efectivo; Transferencia"

Entrada: "Debito", "Credito", "Efectivo" o "Transferencia"; Salida: "Siguiente, agregue como el costo del servicio debe ser calculado: Por hora, A termino."

Entrada: "Por hora" o "A termino"; Salida: "Para terminar, agregue el costo del servicio total en pesos."

Entrada: Un numero mayor que 0; Salida: "El servicio esta siendo creado. Digame cuando quiere continuar."

Entrada: "Next"; Salida: "El servicio fue creado exitosamente con los siguientes parametros. Descripcion: {descripcion}, Categoria: {categoria}, Tipo de pago: {tipo de pago}, Costo: {costo}"

El bot agrega el servicio creado a la lista de servicios del sistema.
## Remover Servicio
Limitacion: Solo accesible por Administradores

Entrada: "remover servicio", "borrar servicio" o "remover un servicio"; Salida: "Por favor, elija el ID del servicio que desea remover."

Entrada: Id del servicio a remover; Salida: "Por favor, explique el motivo por el que se esta removiendo el servicio."

Entrada: Una explicacion no vacia de por que se remueve el servicio; Salida: "El servicio ha sido removido con exito."

El bot remueve el servicio de la lista de servicios del sistema y notifica al Trabajador proveedor del mismo a traves de las notificaciones del sistema.
## Leer Notificaciones
Entrada: "ver notificaciones", "leer notificaciones" o "tengo notificaciones"; Salida: "Cuales notificaciones quiere ver? Leidas, No leidas o Todas"

Entrada: "Todas"; Salida: "Buscando todas sus notificaciones. Digame cuando quiera seguir."

Entrada: "Next"; Salida: Lista de notificaciones del usuario o "No tiene notificaciones."

El bot marca las notificaciones mostradas como leidas.
## Buscar Servicio por Categoria
Limitacion: Solo accesible por Empleadores

Entrada: "buscar servicio", "buscar un servicio"; Salida: "Por favor, inserte la categoria por la que se buscara el servicio."

Entrada: Una categoria que existe en el sistema; Salida: "Desea ordernar el resultado de busqueda?"

Entrada: "no"; Salida: "Buscando servicio. Digame cuando quiere continuar."

Entrada: "Next"; Salida: Lista de servicios con esa categoria, ordenados por fecha de creacion

El bot ordena la lista por cuando cada servicio fue agregado a la misma, por lo tanto 'sin ordenar' en realidad siempre devuelve con los servicios mas viejos primero.
## Buscar Servicio Ordenado por Distancia
Limitacion: Solo accesible por Empleadores

Entrada: "buscar servicio", "buscar un servicio"; Salida: "Por favor, inserte la categoria por la que se buscara el servicio."

Entrada: Una categoria que existe en el sistema; Salida: "Desea ordernar el resultado de busqueda?"

Entrada: "si"; Salida: "Como desea ordenar el resultado, por calificacion del proveedor o distancia entre usted y el proveedor?"

Entrada: "distancia"; Salida: "Buscando servicio. Digame cuando quiere continuar."

Entrada: "Next"; Salida: Lista de servicios con esa categoria, ordenados por la distancia del trabajador al empleador

El bot calcula la distancia en base a las direcciones del empleador y trabajador.
## Buscar Usuario por Email
Entrada: "buscar usuario" o "buscar un usuario"; Salida: "Quiere buscar por ID del usuario o por su email?"

Entrada: "email"; Salida: "Inserte email del usuario que quiere buscar."

Entrada: Email de un usuario del sistema; Salida: "Buscando usuario. Digame cuando quiere continuar."

Entrada: "Next"; Salida: Informacion del usuario buscado
## Buscar Usuario por ID
Entrada: "buscar usuario" o "buscar un usuario"; Salida: "Quiere buscar por ID del usuario o por su email?"

Entrada: "Id"; Salida: "Inserte el Id del usuario que quiere buscar."

Entrada: Id de un usuario del sistema; Salida: "Buscando usuario. Digame cuando quiere continuar."

Entrada: "Next"; Salida: Informacion del usuario buscado
## Calificar Contrato
Limitacion: Solo accesible por Empleadores y Trabajadores

Entrada: "evaluar contrato", "evaluar un contrato" o "calificar un contrato"; Salida: "Validando Usuario. Dime cuando quieras continuar."

Entrada: "Next"; Salida: "Ingrese un numero de id del contrato que desea evaluar."

Entrada: ID del contrato; Salida: "Ingrese un numero de 1 a 5, para calificar."

Entrada: Un numero del 1 al 5; Salida: "Se evaluó el servicio con exito."

El bot crea las calificaciones del usuario opuesto - el Trabajador califica al Empleador y viceversa - y las agrega al contrato, la lista de calificaciones del usuario, calcula la calificacion total y envia una notificacion a ambos usuarios involucrados en el contrato.
## Buscar Servicio Ordenado por Calificacion
Limitacion: Solo accesible por Empleadores

Entrada: "buscar servicio", "buscar un servicio"; Salida: "Por favor, inserte la categoria por la que se buscara el servicio."

Entrada: Una categoria que existe en el sistema; Salida: "Desea ordernar el resultado de busqueda?"

Entrada: "si"; Salida: "Como desea ordenar el resultado, por calificacion del proveedor o distancia entre usted y el proveedor?"

Entrada: "calificacion"; Salida: "Buscando servicio. Digame cuando quiere continuar."

Entrada: "Next"; Salida: Lista de servicios con esa categoria, ordenados por la calificacion del trabajador
## Despedida
Entrada: "chau", "adios", "eso es todo"; Salida: "Gracias por usar nuestros servicios. Que tengas buen dia!"
## Cancelar
Entrada: "cancelar"; Salida: "Cancelando operacion."

El comando "cancelar" actua como comodin en cualquier escenario para cortar el comando y resetear cualquier estado en el que este el bot. Sirve como escape redudante en el caso del que bot se detenga por una excepcion u otro error.
# Conclusion
## Areas de Mejora
Se recalcan a continuacion ciertos puntos que el equipo reconoce podria mejorarse, pero a falta de tiempo material entre instancias de feedback y la fecha final de entrega:

### Casos de Prueba Individuales
Debido al testing fue realizado en un modelo "Testing First" para el testing de cada clase, algunas clases de prueba utilizan de forma ineficiente los metodos construidos mas tarde en la implementacion. En particular se reconoce la falta de clases de testing para las clases de utilidad como los Publishers y Autenticador.

### Excepciones
Si bien las excepciones fueron aplicadas, se reconoce la posiblidad de mejora al combinarlas con la experiencia del usuario. Ahora mismo el programa corta funcionamiento al lanzar una excepcion, que se vuelve visible para el operador desde la consola pero no al usuario de Telegram.

## CRCs y UML
Adjunto, la carpeta de Docs, se encuentran las tarjetas CRC del programa junto con su diagrama UML. Se recalca la separacion del diagrama UML en las capas mencionadas debido a lo dificil de leer que se vuelve de conectarlo completamente a nivel de cada clase. A su vez, en la carpeta Docs_Old se encuentran las iteraciones anteriores de las CRC y UML, por referencia.

## Observaciones
Resumiendo la experiencia del proyecto, se separan las observaciones en desafios, lecciones aprendidas y recursos utilizados.
### Desafios
Se hace enfasis en la complejidad que el equipo encontro en el manejo de APIs, en particular del proceso de debuggear metodos que las utilizaban. Expandiendo esto, se encontro como dificultad el testing de clases que utilizan otras librerias, principalmente debido a la incapacidad de ver como estas interactuaban con el codigo paso a paso.

Otro desafio recalcable es el mantener la aplicacion de OCP y Low Coupling con cada iteracion del proyecto. Al expandirse el alcance del mismo y agregar mas funcionalidades, el evitar que las clases estuvieran saturadas de relaciones y dependencias se volvio mas y mas complejo.

### Lecciones Aprendidas
Se resalta como aprendizaje la dificultad de distribuir el trabajo de forma efectiva con la cantidad de interrelaciones entre clases. La cantidad de dependencias de las mismas, en particular una vez que se volvio necesario implementar el motor del programa, hizo dificil mantener el acercamiento tradicional de dar a cada clase un encargado en el equipo.

Tambien es valido mencionar el aprendizaje en el uso de GitHub y como la separacion por branches y el proceso de revision de conflictos ayudo a simplificar el trabajo en equipo y la distribucion del mismo.

Se recalca el aprendizaje de como aplicar diferentes patrones de diseño, que hizo necesario el estudiarlos mas a fondo y el pensar como podrian ser de utilidad para diferentes funcionalidades del programa.

### Recursos Utilizados
Se hizo uso principalmente de [Refactoring Guru](https://refactoring.guru/es/) para la investigacion y aplicacion de los patrones de diseño, la [documentacion de Microsoft sobre dotnet](https://learn.microsoft.com/en-us/dotnet/) para la busqueda de referencias y metodos estandar para ser utilizados y reducir la creacion de codigo redundante, y el [sitio de Telegram para referencias de sus bots](https://core.telegram.org/).

Tambien se hizo uso de [Stack Overflow](https://stackoverflow.com/) para busqueda de errores particulares y formas de hacer uso de los metodos y atributos estadar de C#.
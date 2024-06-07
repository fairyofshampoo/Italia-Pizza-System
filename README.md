# Sistema de Gestión Italia Pizza

## Descripción del Proyecto

La empresa "Italia Pizza" requiere la implementación de un sistema de gestión de información para manejar operaciones de productos, pedidos, usuarios y catálogos generales. Actualmente, no cuentan con un sistema de información y la gestión se realiza mediante registros en papel, lo que lleva a un control deficiente y un tiempo excesivo dedicado a la conciliación y seguimiento de pedidos. El desarrollo de la solución debe realizarse utilizando tecnologías actuales con un diseño de base de datos optimizado para evitar almacenar información innecesaria. Además, el diseño de la interfaz de usuario debe permitir que el sistema opere con dispositivos de entrada tradicionales (teclado y ratón) así como dispositivos de entrada táctil.

El nuevo sistema está destinado a desarrollarse bajo una arquitectura cliente-servidor e instalarse localmente como software de escritorio que se conecta a una base de datos relacional. Las tecnologías propuestas para el nuevo sistema son: WPF (Windows Presentation Foundation) para la capa de aplicación y Microsoft SQL Server para la capa de datos.

## Características

- **Módulo de Usuarios:** Registro, edición y eliminación de usuarios. Funcionalidad de búsqueda por nombre, dirección o número de teléfono. Autenticación de inicio de sesión.
- **Módulo de Productos:** Registro, edición y eliminación de productos. Gestión de inventario, incluida la generación de informes de inventario en formato PDF. Validación de inventario para detectar faltantes o excedentes.
- **Módulo de Pedidos:** Colocación, edición y cambio de estado de pedidos. Funcionalidad de búsqueda por usuario, fecha o estado.
- **Módulo de Cocina:** Ver y gestionar pedidos para preparar. Gestionar recetas para cada plato.
- **Módulo Financiero:** Registrar ingresos y gastos, incluido el cálculo del saldo diario. Gestionar proveedores y pedidos de productos.

## Tecnologías Utilizadas

- **.NET Framework:** Proporciona un entorno de desarrollo para construir y ejecutar aplicaciones de escritorio.
- **Entity Framework:** Marco de mapeo objeto-relacional utilizado para interactuar con la base de datos.
- **WPF (Windows Presentation Foundation):** Marco para construir aplicaciones de escritorio con interfaces de usuario ricas.
- **SQL Server:** Sistema de gestión de bases de datos relacionales para almacenamiento de datos.

## Instalación

1. Clonar el repositorio:

```bash
git clone https://github.com/fairyofshampoo/ItaliaPizzaManagementSystem.git
```

2. Abrir el proyecto en Visual Studio.

3. Configurar la cadena de conexión a la base de datos en la configuración de la aplicación.

4. Compilar y ejecutar la aplicación.

Los credenciales para acceder son:
    **MESERO**
    -username: monisss
    -password: @Michelle1

    **GERENTE**
    -username: lalin
    -password: @Michelle1

    **COCINERO**
    -username: mirinda
    -password: @Michelle1

    **CAJERO**
    -username: michi
    -password: @Michelle1

- Este proyecto se desarrolla como parte de un curso de desarrollo de software.
- Última actualización: [06/06/2024]
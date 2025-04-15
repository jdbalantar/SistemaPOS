# 🧾 SistemaPOS

**SistemaPOS** es una aplicación de punto de venta moderna desarrollada con **.NET MAUI**, orientada a pequeñas y medianas empresas que necesitan gestionar ventas, clientes, productos y facturación. Implementa un sistema de fidelización con puntos redimibles y generación de facturas en PDF. La arquitectura del proyecto está basada en **CQRS**, **MVVM** y una separación clara de capas.

---

## 🧰 Tecnologías

- ✅ [.NET 8](https://dotnet.microsoft.com/)
- ✅ [MAUI](https://learn.microsoft.com/dotnet/maui/)
- ✅ [MediatR (CQRS)](https://github.com/jbogard/MediatR)
- ✅ [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- ✅ [SQLite](https://www.sqlite.org/index.html)
- ✅ [PDFSharpCore](https://github.com/ststeiger/PdfSharpCore)
- ✅ [MVVM Pattern](https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/)
- ✅ [Microsoft.Extensions.DependencyInjection](https://learn.microsoft.com/dotnet/core/extensions/dependency-injection)

---

## ✨ Funcionalidades

- 🧾 Gestión de productos
- 👥 Gestión de clientes con puntos de fidelización
- 🛒 Carrito de compras
- 💳 Checkout con método de pago y validaciones
- 📄 Generación de facturas en PDF
- 📚 Historial de facturas con descarga individual
- 🔐 Autenticación de usuarios
- 📊 Panel principal de navegación estilo dashboard

---

## 🗂️ Estructura del Proyecto

```
SistemaPOS/
├── ApplicationLayer/
│   ├── DTOs/
│   ├── Features/
│   ├── Interfaces/
│   └── Validators/
├── Domain/
│   ├── Entities/
│   └── Interfaces/
├── Infrastructure/
│   ├── Persistence/
│   └── Identity/
├── SistemaPOS/
│   ├── Views/
│   ├── ViewModels/
│   └── MainPage.xaml
```

---

## ⚙️ Requisitos

- Visual Studio 2022 (con .NET MAUI workload instalado)
- .NET 8 SDK
- Windows 10/11 (para entorno MAUI Desktop)
- SQLite (incluido localmente, no requiere instalación)

---

## 🚀 Instrucciones para Ejecutar

1. **Clonar el repositorio:**

   ```bash
   git clone https://github.com/jdbalantar/SistemaPOS.git
   cd SistemaPOS
   ```

2. **Restaurar los paquetes NuGet:**

   ```bash
   dotnet restore
   ```

3. **Aplicar migraciones y crear la base de datos:**

   *(si usas EF Core Migrations)*

   ```bash
   dotnet ef database update
   ```

4. **Ejecutar la aplicación:**

   ```bash
   dotnet build
   dotnet run
   ```

---

## 📷 Capturas de Pantalla

> Próximamente: imágenes del dashboard, carrito, checkout y factura PDF.

---

## 🧠 Arquitectura

- **CQRS con MediatR** para separar comandos y consultas.
- **MVVM (Model-View-ViewModel)** para separar la lógica de UI y facilitar el testing.
- **Inyección de dependencias** con Microsoft.Extensions.DependencyInjection.
- **XAML + MAUI** para un diseño visual multiplataforma.

---

## 📦 TODO / Futuras Mejoras

- Reportes gráficos
- Dashboard estadístico
- Integración con dispositivos físicos (impresoras, lectores)
- Firma digital de facturas
- Exportar historial a Excel

---

## 📄 Licencia

Este proyecto se distribuye bajo licencia MIT.

---

Credenciales de ingreso:
 ```bash
Usuario: admin@possystem.com
Contraseña: Admin123#$
 ```

## 👨‍💻 Autor

Desarrollado por [Juan David Balanta Rentería](https://github.com/jdbalantar) – 2025

# HISTORIA DE USUARIO — HU-10

**Código del Proyecto/Requerimiento:** ECO120

**Nombre del Proyecto/Requerimiento:** Sistema Ecommerce Corporación Textil Salas SAC

**Rol:** Product Owner: Franco Quispe Pereyra

**Origen del requerimiento:** Necesidad de negocio

**Código de la historia:** HU-10

**Nombre de la historia:** Reportes administrativos

**Versión de la historia:** 1.0

**Fecha de Declaración:** 29/03/2025

**Prioridad:** Alta

**Estimación de Esfuerzo:** 5

**Dependencias:** Ninguna explícita (evaluar integraciones con datos de ventas/pedidos/productos)

---

## Declaración

YO COMO administrador del sistema, QUIERO generar y visualizar reportes de ventas, pedidos y productos, PARA analizar el desempeño del negocio y apoyar la toma de decisiones.

---

## Criterios de Aceptación

1. DADO QUE el administrador accede al módulo de reportes, CUANDO seleccione el tipo de reporte (ventas, pedidos o productos), ENTONCES el sistema debe generar la información correspondiente.

2. DADO QUE el administrador desea analizar información específica, CUANDO seleccione un rango de fechas u otros filtros (por ejemplo: categoría, estado de pedido, cliente), ENTONCES el sistema debe mostrar los datos filtrados correctamente.

3. DADO QUE el reporte ha sido generado, CUANDO se muestre en pantalla, ENTONCES el sistema debe presentar la información de forma clara y organizada (tablas, totales, gráficos opcionales).

4. DADO QUE el administrador visualiza un reporte, CUANDO seleccione la opción de exportar, ENTONCES el sistema debe permitir descargar el reporte en PDF o Excel.

5. DADO QUE existen nuevas ventas o cambios en el sistema, CUANDO el administrador genere un reporte, ENTONCES la información debe reflejar datos actualizados (consistencia con la base de datos en tiempo de consulta).

---

## Detalles / Notas de la Conversación

- Incluir filtros recomendados: rango de fechas, categoría de producto, estado del pedido, cliente/empresa, canal de venta.
- Diseñar la UI para mostrar: encabezado del reporte (tipo, periodo, filtros aplicados), tabla con filas detalladas, subtotales y totales, y botones de exportación.
- Evaluar rendimiento y paginación para grandes volúmenes de datos.
- Determinar formato y contenido exacto de exportaciones (columnas requeridas para Excel/PDF).

---

## Prototipo(s) / Siguientes pasos sugeridos

- Diseñar mockups de la pantalla de reports (listado, filtros, vista detalle, exportación).
- Implementar endpoint(s) en backend: `/Admin/Reports` con parámetros de filtro y formato (`json`, `pdf`, `excel`).
- Crear vistas/acciones en `Controllers/AdminController.cs` o nuevo `ReportsController` y vistas en `Views/Admin/Reports/`.
- Añadir pruebas básicas de integración que verifiquen generación y exportación de reportes.

---

Si quieres, puedo: 1) generar el mockup HTML/CSS para la vista de reportes, o 2) crear el endpoint y la vista inicial en el proyecto ASP.NET existente. ¿Cuál prefieres que haga ahora?

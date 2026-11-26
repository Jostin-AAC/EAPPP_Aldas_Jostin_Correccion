# EAPPP_Aldas_Jostin

Aplicación de escritorio en Windows Forms (C#) para gestionar pedidos de la cafetería universitaria con catálogo de productos, validaciones y cálculo automático de totales.

## Datos del estudiante
- **Estudiante:** Aldas Jostin
- **Asignatura:** Diseño y Programación de Software (ITID 543)
- **Docente:** Ing. José Naranjo
- **Institución:** Escuela Politécnica Nacional (EPN)

## Cómo ejecutar el proyecto
1. Clonar o descargar este repositorio.
2. Abrir el archivo de solución (`.slnx` / `.sln`) en Visual Studio.
3. Presionar **F5** (o el botón **Iniciar**) para compilar y ejecutar el formulario.

## Eventos implementados
1. **Load (`frmPedidoCafeteria_Load`):** Crea las columnas de `lvCatalogo` y `lvPedido`, carga los 10 productos con sus precios e inicializa los controles.
2. **SelectedIndexChanged (`lvCatalogo_SelectedIndexChanged` y `lvPedido_SelectedIndexChanged`):** Muestra el producto elegido en `txtProducto`, actualiza `lblEstado` y controla si `btnAgregar` y `btnQuitar` están habilitados.
3. **Validating (`txtCantidad_Validating`):** Comprueba con `int.TryParse` que la cantidad sea un número entero mayor o igual a 1; si no, cancela el cambio de foco y muestra un error en `lblEstado`.
4. **TextChanged (`txtCantidad_TextChanged` y `txtProducto_TextChanged`):** Revisa mientras se escribe que haya un producto seleccionado y una cantidad válida para habilitar `btnAgregar`.
5. **CheckedChanged (`rbtnEntrega_CheckedChanged`, `chkPostre_CheckedChanged`, `chkServilletas_CheckedChanged`):** Llama a `ActualizarTotal()` para sumar o quitar el recargo de entrega y el postre, e informa el cambio en `lblEstado`.
6. **Click (`btnAgregar_Click`, `btnQuitar_Click`, `btnLimpiar_Click`):** Valida el cliente y los datos, agrega ítems con su subtotal a `lvPedido`, elimina filas seleccionadas y reinicia todo el pedido a `$0.00`.

## Reglas de negocio
- El producto no se digita manualmente (`txtProducto` es solo lectura); se elige desde `lvCatalogo`.
- **Subtotal por ítem:** `Cantidad * Precio unitario`.
- **Total general:** suma de los subtotales de `lvPedido` + `$0.50` si se marca entrega en aula + `$1.00` si se marca postre del día.
- No permite agregar ítems si falta el cliente, el producto o si la cantidad no es un entero mayor o igual a 1.
- El cálculo del total está centralizado en un solo método (`ActualizarTotal`) para no repetir código.
// Examen Práctico Acumulativo Primer Parcial
// Estudiante: Aldas Jostin
// Materia: Diseño y Programación de Software (ITID 543)
// Fecha: 25/11/2026

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace EAPPP_Aldas_Jostin_Correccion
{
    public partial class frmPedidoCafeteria : Form
    {
        // Catálogo oficial de productos y precios
        string[] productos = {
            "Empanada de queso",
            "Sánduche de pollo",
            "Bolón de verde mixto",
            "Humita tradicional",
            "Tostada con queso",
            "Café pasado",
            "Chocolate caliente",
            "Jugo natural (naranja/mora)",
            "Batido de fruta",
            "Botella de agua"
        };

        double[] precios = { 1.25, 2.50, 2.25, 1.50, 1.00, 0.85, 1.35, 1.50, 2.00, 0.75 };

        double precioSeleccionado = 0;

        public frmPedidoCafeteria()
        {
            InitializeComponent();
        }

        // Evento Load: configura las listas y carga los 10 productos iniciales
        private void frmPedidoCafeteria_Load(object sender, EventArgs e)
        {
            lvCatalogo.View = View.Details;
            lvCatalogo.FullRowSelect = true;
            lvCatalogo.Columns.Clear();
            lvCatalogo.Columns.Add("Producto", 185);
            lvCatalogo.Columns.Add("Precio", 75);

            lvCatalogo.Items.Clear();
            for (int i = 0; i < productos.Length; i++)
            {
                ListViewItem fila = new ListViewItem(productos[i]);
                fila.SubItems.Add("$" + precios[i].ToString("0.00"));
                lvCatalogo.Items.Add(fila);
            }

            lvPedido.View = View.Details;
            lvPedido.FullRowSelect = true;
            lvPedido.Columns.Clear();
            lvPedido.Columns.Add("Producto", 150);
            lvPedido.Columns.Add("Cant.", 50);
            lvPedido.Columns.Add("Subtotal", 76);

            txtProducto.ReadOnly = true;
            txtCantidad.Text = "1";
            rbtnMostrador.Checked = true;
            chkPostre.Checked = false;
            chkServilletas.Checked = false;
            btnAgregar.Enabled = false;
            btnQuitar.Enabled = false;
            lblTotal.Text = "Total: $0.00";
            lblEstado.Text = "Estado: Seleccione un producto.";
        }

        // Evento SelectedIndexChanged del catálogo: carga el producto y su precio
        private void lvCatalogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvCatalogo.SelectedItems.Count > 0)
            {
                int indice = lvCatalogo.SelectedIndices[0];
                txtProducto.Text = productos[indice];
                precioSeleccionado = precios[indice];

                int cantidad;
                if (int.TryParse(txtCantidad.Text, out cantidad) && cantidad >= 1)
                {
                    btnAgregar.Enabled = true;
                }
                else
                {
                    btnAgregar.Enabled = false;
                }

                lblEstado.Text = "Estado: " + productos[indice] + " ($" + precioSeleccionado.ToString("0.00") + ") seleccionado.";
            }
            else
            {
                txtProducto.Clear();
                precioSeleccionado = 0;
                btnAgregar.Enabled = false;
            }
        }

        // Evento SelectedIndexChanged del pedido: habilita o deshabilita el botón Quitar
        private void lvPedido_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvPedido.SelectedItems.Count > 0)
            {
                btnQuitar.Enabled = true;
            }
            else
            {
                btnQuitar.Enabled = false;
            }
        }

        // Evento Validating: comprueba que la cantidad sea un entero >= 1 al salir del campo
        private void txtCantidad_Validating(object sender, CancelEventArgs e)
        {
            int cantidad;
            if (!int.TryParse(txtCantidad.Text, out cantidad) || cantidad < 1)
            {
                e.Cancel = true;
                lblEstado.Text = "Error: La cantidad debe ser un entero mayor o igual a 1.";
                txtCantidad.SelectAll();
            }
        }

        // Evento TextChanged en cantidad: valida en tiempo real para habilitar btnAgregar
        private void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            int cantidad;
            if (!string.IsNullOrWhiteSpace(txtProducto.Text) && int.TryParse(txtCantidad.Text, out cantidad) && cantidad >= 1)
            {
                btnAgregar.Enabled = true;
            }
            else
            {
                btnAgregar.Enabled = false;
            }
        }

        // Evento TextChanged en producto: valida en tiempo real para habilitar btnAgregar
        private void txtProducto_TextChanged(object sender, EventArgs e)
        {
            int cantidad;
            if (!string.IsNullOrWhiteSpace(txtProducto.Text) && int.TryParse(txtCantidad.Text, out cantidad) && cantidad >= 1)
            {
                btnAgregar.Enabled = true;
            }
            else
            {
                btnAgregar.Enabled = false;
            }
        }

        // Evento CheckedChanged: actualiza el total cuando cambia la modalidad de entrega
        private void rbtnEntrega_CheckedChanged(object sender, EventArgs e)
        {
            ActualizarTotal();
            if (rbtnEntrega.Checked)
            {
                lblEstado.Text = "Opción aplicada: Entrega en aula (+$0.50).";
            }
            else
            {
                lblEstado.Text = "Opción aplicada: Retiro en mostrador.";
            }
        }

        // Evento CheckedChanged: actualiza el total al marcar o desmarcar postre
        private void chkPostre_CheckedChanged(object sender, EventArgs e)
        {
            ActualizarTotal();
            if (chkPostre.Checked)
            {
                lblEstado.Text = "Opción aplicada: Postre del día (+$1.00).";
            }
            else
            {
                lblEstado.Text = "Opción retirada: Postre del día.";
            }
        }

        // Evento CheckedChanged: notifica el cambio en servilletas/cubiertos
        private void chkServilletas_CheckedChanged(object sender, EventArgs e)
        {
            ActualizarTotal();
            if (chkServilletas.Checked)
            {
                lblEstado.Text = "Opción aplicada: Servilletas / Cubiertos.";
            }
            else
            {
                lblEstado.Text = "Opción retirada: Servilletas / Cubiertos.";
            }
        }

        // Evento Click en Agregar: valida datos, calcula subtotal y añade fila al pedido
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCliente.Text))
            {
                lblEstado.Text = "Error: Ingrese el nombre del cliente.";
                txtCliente.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtProducto.Text))
            {
                lblEstado.Text = "Error: Seleccione un producto del catálogo.";
                return;
            }

            int cantidad;
            if (!int.TryParse(txtCantidad.Text, out cantidad) || cantidad < 1)
            {
                lblEstado.Text = "Error: La cantidad debe ser un entero mayor o igual a 1.";
                txtCantidad.Focus();
                txtCantidad.SelectAll();
                return;
            }

            double subtotal = cantidad * precioSeleccionado;

            ListViewItem item = new ListViewItem(txtProducto.Text);
            item.SubItems.Add(cantidad.ToString());
            item.SubItems.Add("$" + subtotal.ToString("0.00"));
            lvPedido.Items.Add(item);

            ActualizarTotal();
            txtCantidad.Text = "1";
            lblEstado.Text = "Estado: '" + txtProducto.Text + "' agregado (Cant: " + cantidad + ", Subtotal: $" + subtotal.ToString("0.00") + ").";
        }

        // Evento Click en Quitar: elimina el ítem seleccionado y recalcula el total
        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (lvPedido.SelectedItems.Count > 0)
            {
                string nombre = lvPedido.SelectedItems[0].Text;
                lvPedido.Items.Remove(lvPedido.SelectedItems[0]);
                btnQuitar.Enabled = false;
                ActualizarTotal();
                lblEstado.Text = "Estado: '" + nombre + "' eliminado del pedido.";
            }
        }

        // Evento Click en Limpiar: reinicia todos los controles a su estado inicial
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            lvPedido.Items.Clear();
            txtCliente.Clear();
            txtProducto.Clear();
            lvCatalogo.SelectedItems.Clear();
            precioSeleccionado = 0;
            txtCantidad.Text = "1";
            rbtnMostrador.Checked = true;
            chkPostre.Checked = false;
            chkServilletas.Checked = false;
            btnAgregar.Enabled = false;
            btnQuitar.Enabled = false;

            ActualizarTotal();
            lblEstado.Text = "Estado: Pedido reiniciado.";
            txtCliente.Focus();
        }

        // Método único para calcular y mostrar el total general (DRY)
        private void ActualizarTotal()
        {
            double total = 0;

            foreach (ListViewItem item in lvPedido.Items)
            {
                string textoSubtotal = item.SubItems[2].Text.Replace("$", "");
                total += Convert.ToDouble(textoSubtotal);
            }

            if (rbtnEntrega.Checked)
            {
                total += 0.50;
            }

            if (chkPostre.Checked)
            {
                total += 1.00;
            }

            lblTotal.Text = "Total: $" + total.ToString("0.00");
        }
    }
}

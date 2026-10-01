namespace Prana.Presentacion;

// Un único desplazamiento para la página; los campos conservan su tamaño legible.
internal static class DistribucionAdaptable
{
    internal static FlowLayoutPanel Fila(params Control[] controles)
    {
        var fila = new FlowLayoutPanel { AutoSize = false, WrapContents = true,
            Margin = new Padding(0), Padding = new Padding(8), Dock = DockStyle.Top };
        foreach (var control in controles)
        {
            control.Dock = DockStyle.None;
            control.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            control.Margin = new Padding(4, 4, 12, 4);
            fila.Controls.Add(control);
        }
        return fila;
    }

    internal static Panel Campo(Label etiqueta, Control entrada, int ancho = 190)
    {
        var panel = new Panel { Width = ancho, Height = 58 };
        etiqueta.Dock = DockStyle.Top;
        etiqueta.AutoSize = false;
        etiqueta.Height = 23;
        entrada.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        entrada.Dock = DockStyle.None;
        entrada.SetBounds(0, 26, ancho, entrada.Height);
        panel.Controls.Add(entrada);
        panel.Controls.Add(etiqueta);
        return panel;
    }

    internal static void Ancho(Control control, int margen = 20)
    {
        var padre = control.Parent!;
        control.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        void Ajustar() => control.Width = Math.Max(100, padre.ClientSize.Width - control.Left - margen);
        padre.SizeChanged += (_, _) => Ajustar();
        Ajustar();
    }

    internal static void Pagina(Form formulario, params Control[] secciones)
    {
        formulario.SuspendLayout();
        formulario.AutoScroll = false;
        formulario.AutoScrollMinSize = Size.Empty;
        formulario.BackColor = EstiloGestion.Fondo;
        var pagina = new Panel { Dock = DockStyle.Fill, AutoScroll = true,
            BackColor = EstiloGestion.Fondo, ForeColor = formulario.ForeColor, Name = "paginaAdaptable" };
        foreach (Control anterior in formulario.Controls) anterior.Hide();
        int orden = 0;
        foreach (var seccion in secciones)
        {
            seccion.TabIndex = orden++;
            seccion.Dock = DockStyle.None;
            seccion.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            if (seccion is ScrollableControl scroll) scroll.AutoScroll = false;
            pagina.Controls.Add(seccion);
            seccion.Show();
        }
        bool ajustando = false;
        void Ajustar()
        {
            if (ajustando) return;
            ajustando = true;
            pagina.SuspendLayout();
            try
            {
                int anchoMinimo = (int)Math.Ceiling(740 * pagina.DeviceDpi / 96.0);
                int ancho = Math.Max(anchoMinimo, pagina.Width - SystemInformation.VerticalScrollBarWidth - 16);
                int y = 8;
                var desplazamiento = pagina.AutoScrollPosition;
                foreach (var seccion in secciones)
                {
                    int alto = seccion is FlowLayoutPanel fila
                        ? fila.GetPreferredSize(new Size(ancho, 0)).Height : seccion.Height;
                    seccion.SetBounds(8 + desplazamiento.X, y + desplazamiento.Y, ancho, alto);
                    y += seccion.Height + 8;
                }
                pagina.AutoScrollMinSize = new Size(ancho + 16, y);
            }
            finally
            {
                pagina.ResumeLayout(true);
                ajustando = false;
            }
        }
        pagina.SizeChanged += (_, _) => Ajustar();
        foreach (var seccion in secciones) seccion.SizeChanged += (_, _) => Ajustar();
        formulario.Controls.Add(pagina);
        pagina.BringToFront();
        formulario.ResumeLayout(true);
        Ajustar();
    }
}

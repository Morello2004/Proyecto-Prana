using System.Globalization;

namespace Prana.Presentacion
{
    // Estilos exclusivos de presentación, compartidos por Pagos y Entregas.
    internal static class EstiloGestion
    {
        internal static readonly Color Fondo = Color.FromArgb(13,19,27);
        internal static readonly Color Panel = Color.FromArgb(21,30,41);
        internal static void Grilla(DataGridView grid)
        {
            grid.AutoGenerateColumns=false;
            grid.ReadOnly=true;
            grid.AllowUserToAddRows=false;
            grid.AllowUserToDeleteRows=false;
            grid.MultiSelect=false;
            grid.SelectionMode=DataGridViewSelectionMode.FullRowSelect;
            grid.RowHeadersVisible=false;
            grid.BackgroundColor=Fondo;
            grid.ForeColor=Panel;
            grid.BorderStyle=BorderStyle.None;
            grid.EnableHeadersVisualStyles=false;
            grid.ColumnHeadersHeight=38;
            grid.ColumnHeadersHeightSizeMode=DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid.ColumnHeadersDefaultCellStyle.BackColor=Color.FromArgb(37,52,70);
            grid.ColumnHeadersDefaultCellStyle.ForeColor=Color.White;
            grid.DefaultCellStyle.ForeColor=Panel;
            grid.DefaultCellStyle.BackColor=Color.White;
            grid.DefaultCellStyle.SelectionBackColor=Color.FromArgb(35,100,166);
            grid.DefaultCellStyle.SelectionForeColor=Color.White;
            grid.DefaultCellStyle.Padding=new Padding(4);
            grid.AlternatingRowsDefaultCellStyle.BackColor=Color.FromArgb(236,242,249);
            grid.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;
            grid.AutoSizeRowsMode=DataGridViewAutoSizeRowsMode.AllCells;
            grid.DefaultCellStyle.WrapMode=DataGridViewTriState.True;
        }
        internal static void Columna(DataGridView grid,string propiedad,string titulo,int peso=100,string formato="")
        {
            grid.Columns.Add(new DataGridViewTextBoxColumn { Name=propiedad,DataPropertyName=propiedad,HeaderText=titulo,FillWeight=peso,
                DefaultCellStyle=new DataGridViewCellStyle { Format=formato,FormatProvider=CultureInfo.GetCultureInfo("es-AR") } });
        }
    }
}

using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Save_Our_Pets.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Save_Our_Pets.Controllers
{
    public class GenerarPdfController : Controller
    {
        //modelos a usar
        save_our_petsEntities db = new save_our_petsEntities();
        ImagenModel modelImg = new ImagenModel();

        //tamaño de los márgenes
        float margenSup = 75;
        float margenDer = 35;
        float margenInf = 70;
        float margenIzq = 35;

        // GET: GenerarPdf
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pdf(string n)
        {
            /*if (n =="nuevo")
            {
                return RedirectToAction("Index", "Perfil", null);
            }*/

            //con este string verificamos si quiere un pdf general o un específico
            string[] url = n.Split('/');
            int id_especifico = 0;
            int existe = 0;

            //titulo y subtitulo
            string titulo = "Reporte de Productos";
            string subtitulo = "Productos en existencia";

            MemoryStream ms = new MemoryStream();
            
            PdfWriter pw = new PdfWriter(ms);
            PdfDocument pdfDocument = new PdfDocument(pw);

            Document doc = new Document(pdfDocument, PageSize.LETTER);//nuevo documento con hojas tamaño Carta
            doc.SetMargins(margenSup, margenDer, margenInf, margenIzq);//margenes del documento

            //detalles de la imagen para el header
            string pathLogo = Server.MapPath("~/Content/images/logo_for_pdf.png");
            Image img = new Image(ImageDataFactory.Create(pathLogo));

            //por medio de eventos sabremos si es para el encabezado o el pie de página
            pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, new HeaderEventHandler1(img,doc));
            pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, new FooterEventHandler1(doc));


            string nameFont = Server.MapPath("/Content/fonts/montserrat-light.ttf");//buscamos una fuente que deseamos usar
            PdfFont font = PdfFontFactory.CreateFont(nameFont);//creamos una fuente de tipo PdfFont con la fuente que buscamos

            //creamos un estilo para luego solo agregarlo
            Style styles = new Style()
                .SetFontSize(24)
                .SetFont(font)
                .SetFontColor(ColorConstants.BLUE)
                .SetBackgroundColor(ColorConstants.PINK)
                ;

            //creamos la primera tabla que tendrá el titulo del reporte
            Table table = new Table(1).UseAllAvailableWidth();//usamos todo el ancho de la hoja

            //por medio de un if controlaremos que se agregará al pdf

            if (url[0] == "usuarios")
            {
                //si es en general
                titulo = "Reporte de usuarios";
                subtitulo = "Usuarios registrados";
                if (url.Count() > 1)
                {
                    id_especifico = int.Parse(url[1]);
                    List<Usuarios> vModel = db.Usuarios.Where(u => u.id_usuario == id_especifico).ToList();
                    existe = vModel.Count;
                    if(existe>0) { //si existe cambia el título y subtítulo en el pdf
                    titulo = "Detalles de Usuario";
                    subtitulo = "";
                    }
                }
            }else if(url[0]== "mascotas")
            {
                //si es en general
                titulo = "Reporte de mascotas";
                subtitulo = "Mascotas registradas";
                if (url.Count() > 1)
                {
                    id_especifico = int.Parse(url[1]);
                    List<Mascota> vModel = db.Mascota.Where(u => u.id_mascota == id_especifico).ToList();
                    existe = vModel.Count;
                    if (existe > 0)
                    { //si existe cambia el título y subtítulo en el pdf
                        titulo = "Detalles de Mascota";
                        subtitulo = "";
                    }
                }
            }else if (url[0] == "razas")
            {
                //si es en general
                titulo = "Reporte de razas";
                subtitulo = "Razas registradas";
            }
            else if (url[0] == "vacunas")
            {
                //si es en general
                titulo = "Lista de vacunas";
                subtitulo = "Vacunas registradas";
            }


            Cell cell = new Cell().Add(new Paragraph(titulo).SetFontSize(14))
                .SetTextAlignment(TextAlignment.CENTER)
                .SetBorder(Border.NO_BORDER);

            table.AddCell(cell);//agregamos la celda a la tabla
            cell = new Cell().Add(new Paragraph(subtitulo))
                .SetTextAlignment(TextAlignment.CENTER)
                .SetBorder(Border.NO_BORDER);
            table.AddCell(cell);

            doc.Add(table);//agregamos la tabla

            //centrado
            Style styloCelda = new Style()
                .SetBackgroundColor(new DeviceRgb(19, 119, 197))
                .SetFontColor(ColorConstants.WHITE)
                .SetTextAlignment(TextAlignment.CENTER);

            //centrado sin fondo
            Style styleBack = new Style()
                .SetTextAlignment(TextAlignment.CENTER)
                        .SetBorder(Border.NO_BORDER);
            //para detalles de usuario
            Style styleOnly = new Style()
                .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER);

            //la 2a tabla contendrá la información que se encuentra en la base de Datos

            Table _table = new Table(1).UseAllAvailableWidth();

            if (url[0] == "usuarios")//reportes de Usuarios
            {
                if (existe > 0)
                {

                    Usuarios usuario = db.Usuarios.Find(id_especifico);
                    direccion_usuario direcc_u = db.direccion_usuario.Where(d => d.id_usuario == id_especifico).FirstOrDefault();
                    List<solicitud_adopcion> solicitud_u = db.solicitud_adopcion.Where(s => s.id_usuario == id_especifico).ToList();
                    List<Adopciones> adopciones = db.Adopciones.Where(a => a.id_usuario == id_especifico).ToList();

                    //construimos los párrafos para que de esta manera solo los agreguemos a las celdas
                    string datos_personales = "Nombre: " + usuario.nombres + " " +
                        usuario.apellidos + "\nDUI: " + usuario.DUI
                        + "\nFecha de Nacimiento: " + usuario.fecha_nacimiento.ToShortDateString() + "\nTeléfono: " + usuario.telefono
                        + "\nEmail: " + usuario.email;
                    string datos_direccion;
                    string datos_solicitudes;
                    string datos_adopciones;

                    //si no tiene dirección significa que no es cliente ya que para administrador y empleado no son datos necesarios al menos con su registro por algun administrador
                    if (direcc_u == null)
                    {
                        datos_direccion = "Dirección: No ha brindado su dirección\nDepartamento: No ha brindado su departamento\nMunicipio: No ha brindado su municipio";
                    }
                    else
                    {
                        if (direcc_u.direccion == null || direcc_u.departamento == null || direcc_u.municipio == null)
                        {
                            datos_direccion = "Dirección: No ha brindado su dirección\nDepartamento: No ha brindado su departamento\nMunicipio: No ha brindado su municipio";
                        }
                        else
                        {
                            datos_direccion = "Dirección: " + direcc_u.direccion + "\nDepartamento: " + direcc_u.departamento + "\nMunicipio: " + direcc_u.municipio;
                        }
                    }
                    if (solicitud_u.Count == 0)
                    {
                        datos_solicitudes = "No ha hecho ninguna solicitud de adopción";
                    }
                    else
                    {
                        datos_solicitudes = "Solicitudes de adopción\n";
                        foreach (var sol in solicitud_u)
                        {

                            datos_solicitudes += "Solicito adoptar un " + sol.Especie.nombre;
                            if (sol.estado_solicitud == 0)
                                datos_solicitudes += ", la cual esta en estado de aprobación.";
                            else
                                datos_solicitudes += ", la cual le fue aprobada.";

                            if (sol.empleo_fijo == 0)
                                datos_solicitudes += "\nHa declarado que NO posee empleo fijo ";
                            else
                                datos_solicitudes += "\nHa declarado que SI posee empleo fijo ";

                            if (sol.requisitos_condiciones == 1)
                                datos_solicitudes += "y que SI acepta los términos y condiciones.";
                            else
                                datos_solicitudes += "y que NO acepta los términos y condiciones.";

                        }
                    }

                    if (adopciones.Count == 0)
                    {
                        datos_adopciones = "No posee ninguna adopción";
                    }
                    else
                    {
                        datos_adopciones = "Adopciones aprobadas\n";
                        foreach (var adop in adopciones)
                        {

                            datos_adopciones += adop.Mascota.nombre_mascota + ", un " + adop.Mascota.Especie.nombre + ".";
                            DateTime fechaVisita = (DateTime)adop.reporte_seguimiento.LastOrDefault().fecha_visita;
                            datos_adopciones += "\n La última visita fue " + fechaVisita.ToShortDateString();

                        }
                    }

                    _table = new Table(2).UseAllAvailableWidth();
                    Cell _cell = new Cell(1, 2).Add(new Paragraph("Datos Personales"));
                    _table.AddHeaderCell(_cell.AddStyle(styloCelda));
                    _cell = new Cell().Add(new Paragraph("\n" + datos_personales + "\n\n"));
                    _table.AddHeaderCell(_cell.AddStyle(styleOnly));
                    _cell = new Cell().Add(new Paragraph("\n" + datos_direccion));
                    _table.AddHeaderCell(_cell.AddStyle(styleOnly));
                    _cell = new Cell(1, 2).Add(new Paragraph("Solicitudes y Adopciones"));
                    _table.AddHeaderCell(_cell.AddStyle(styloCelda));
                    _cell = new Cell(1, 2).Add(new Paragraph("\n" + datos_solicitudes + "\n\n" + datos_adopciones));
                    _table.AddHeaderCell(_cell.AddStyle(styleOnly));

                }
                else
                {
                    _table = new Table(5).UseAllAvailableWidth();//cambiamos la cantidad de columnas a tener en el pdf
                    Cell _cell = new Cell().Add(new Paragraph("#"));
                    _table.AddHeaderCell(_cell.AddStyle(styloCelda));
                    _cell = new Cell().Add(new Paragraph("Nombre"));
                    _table.AddHeaderCell(_cell.AddStyle(styloCelda));
                    _cell = new Cell().Add(new Paragraph("DUI"));
                    _table.AddHeaderCell(_cell.AddStyle(styloCelda));
                    _cell = new Cell().Add(new Paragraph("Teléfono"));
                    _table.AddHeaderCell(_cell.AddStyle(styloCelda));
                    _cell = new Cell().Add(new Paragraph("Tipo"));
                    _table.AddHeaderCell(_cell.AddStyle(styloCelda));

                    //información dentro de la base de datos
                    List<Usuarios> modelUsuario = db.Usuarios.ToList();

                    int x = 0;
                    foreach (var item in modelUsuario)
                    {
                        x++;
                        _cell = new Cell().Add(new Paragraph(x.ToString()));
                        _table.AddCell(_cell);
                        _cell = new Cell().Add(new Paragraph(item.nombres + " " + item.apellidos));
                        _table.AddCell(_cell);
                        _cell = new Cell().Add(new Paragraph(item.DUI.ToString()));
                        _table.AddCell(_cell);
                        _cell = new Cell().Add(new Paragraph(item.telefono.ToString()));
                        _table.AddCell(_cell);
                        _cell = new Cell().Add(new Paragraph(item.Tipo_usuario.nombre_tipo));
                        _table.AddCell(_cell);
                    }
                }
            }
            else if (url[0]== "mascotas") 
            {
                if (existe > 0)
                {
                    Mascota mascota = db.Mascota.Find(id_especifico);
                    int id_imagen = db.img_mascota.Where(i => i.id_mascota == id_especifico).Select(i => i.id_imagen).FirstOrDefault();
                    Estado_salud estado_mascota = db.Estado_salud.Where(em => em.id_mascota == id_especifico).FirstOrDefault();
                    List<Reporte_vacunas> vacunas = db.Reporte_vacunas.Where(v => v.id_mascota == id_especifico).ToList();
                    List<reporte_seguimiento> seguimiento = db.reporte_seguimiento.Where(r => r.Adopciones.id_mascota == id_especifico).ToList();

                    //datos generales de la mascota
                    string datos_generales = "Nombre: " + mascota.nombre_mascota + "\nColor de pelo: " + mascota.color_pelo;


                    if (mascota.fecha_nacimiento != null)//si la fecha de nacimiento es nula no se muestra
                    {
                        datos_generales += "\nFecha de Nacimiento: " + mascota.fecha_nacimiento;
                    }

                    if (mascota.peso > 0)
                    {
                        datos_generales += "\nPeso: " + mascota.peso + " Kg";
                    }
                    else
                    {
                        datos_generales += "\nPeso: Desconocido";
                    }

                    datos_generales += "\nEsterilizado: " + mascota.esterilizado + "\nEspecie: " + mascota.Especie.nombre 
                        + "\nRaza: " + mascota.Raza.nombre + "\nEstado: " + mascota.Estado_mascota.nombre_estado
                        + "\nEstado de Salud: " + estado_mascota.descripcion_salud;

                    // imagen de la mascota
                    string pathImg = "";
                    Image imagen;
                    if (id_imagen != 0)
                    {
                        pathImg = "https://localhost:44314/img_mascota/ImagenMascota/" + id_imagen.ToString();
                        imagen = new Image(ImageDataFactory.Create(pathImg));
                    }
                    else
                    {
                        pathImg = Server.MapPath("~/Content/images/no_image.jpg");
                        imagen = new Image(ImageDataFactory.Create(pathImg));
                    }

                    _table = new Table(5).UseAllAvailableWidth();
                    Cell _cell = new Cell(1, 5).Add(new Paragraph("Datos Generales"));
                    _table.AddHeaderCell(_cell.AddStyle(styloCelda));
                    _cell = new Cell(1,2).Add(imagen.ScaleAbsolute(150,175));//imagen
                    _table.AddCell(_cell.AddStyle(styleOnly));
                    _cell = new Cell(1,3).Add(new Paragraph(datos_generales));
                    _table.AddCell(_cell.AddStyle(styleOnly));


                    //salto
                    _cell = new Cell(1, 5).Add(new Paragraph("\n"));
                    _table.AddCell(_cell.AddStyle(styleBack));

                    if (vacunas.Count > 0)
                    {
                        _cell = new Cell(1, 5).Add(new Paragraph("Vacunas"));
                        _table.AddCell(_cell.AddStyle(styloCelda));
                        //tabla de vacunas
                        _cell = new Cell().Add(new Paragraph("#"));
                        _table.AddCell(_cell.AddStyle(styloCelda));
                        _cell = new Cell(1, 2).Add(new Paragraph("Vacuna"));
                        _table.AddCell(_cell.AddStyle(styloCelda));
                        _cell = new Cell().Add(new Paragraph("Fecha aplicada"));
                        _table.AddCell(_cell.AddStyle(styloCelda));
                        _cell = new Cell().Add(new Paragraph("Próxima vacunación"));
                        _table.AddCell(_cell.AddStyle(styloCelda));


                        int x = 0;
                        foreach (var vac in vacunas)
                        {
                            x++;
                            _cell = new Cell().Add(new Paragraph(x.ToString()));
                            _table.AddCell(_cell.AddStyle(styleOnly).SetBorder(new SolidBorder(ColorConstants.BLACK, 1)));
                            _cell = new Cell(1, 2).Add(new Paragraph(vac.Vacunas.vacuna));
                            _table.AddCell(_cell.AddStyle(styleOnly).SetBorder(new SolidBorder(ColorConstants.BLACK, 1)));
                            DateTime fecha_ap = (DateTime)vac.fecha_aplicada;
                            _cell = new Cell().Add(new Paragraph(fecha_ap.ToShortDateString()));
                            _table.AddCell(_cell.AddStyle(styleOnly).SetBorder(new SolidBorder(ColorConstants.BLACK, 1)));
                            DateTime fecha_nueva = (DateTime)vac.fecha_a_aplicar_nueva;
                            _cell = new Cell().Add(new Paragraph(fecha_nueva.ToShortDateString()));
                            _table.AddCell(_cell.AddStyle(styleOnly).SetBorder(new SolidBorder(ColorConstants.BLACK, 1)));
                        }
                    }

                    //salto
                    _cell = new Cell(1, 5).Add(new Paragraph("\n"));
                    _table.AddCell(_cell.AddStyle(styleBack));

                    if (mascota.Estado_mascota.id_estado == 2)//Si es adoptado creamos el reporte de seguimiento
                    {
                        _cell = new Cell(1, 5).Add(new Paragraph("Reporte de Seguimiento"));
                        _table.AddCell(_cell.AddStyle(styloCelda));
                        //tabla de seguimiento
                        _cell = new Cell().Add(new Paragraph("N°"));
                        _table.AddCell(_cell.AddStyle(styloCelda));
                        _cell = new Cell().Add(new Paragraph("Fecha de visita"));
                        _table.AddCell(_cell.AddStyle(styloCelda));
                        _cell = new Cell(1, 3).Add(new Paragraph("Comentario"));
                        _table.AddCell(_cell.AddStyle(styloCelda));

                        int y = 0;
                        foreach (var seg in seguimiento)
                        {
                            y++;
                            _cell = new Cell().Add(new Paragraph(y.ToString()));
                            _table.AddCell(_cell.AddStyle(styleBack).SetBorder(new SolidBorder(ColorConstants.BLACK, 1)));
                            DateTime fecha_vis = (DateTime)seg.fecha_visita;
                            _cell = new Cell().Add(new Paragraph(fecha_vis.ToShortDateString()));
                            _table.AddCell(_cell.AddStyle(styleOnly).SetBorder(new SolidBorder(ColorConstants.BLACK, 1)));
                            _cell = new Cell(1, 3).Add(new Paragraph(seg.comentario));
                            _table.AddCell(_cell.AddStyle(styleOnly).SetBorder(new SolidBorder(ColorConstants.BLACK, 1)));

                        }
                    }


                }
                else
                {
                    _table = new Table(5).UseAllAvailableWidth();//cambiamos la cantidad de columnas a tener en el pdf
                    Cell _cell = new Cell().Add(new Paragraph("#"));
                    _table.AddHeaderCell(_cell.AddStyle(styloCelda));
                    _cell = new Cell().Add(new Paragraph("Nombre"));
                    _table.AddHeaderCell(_cell.AddStyle(styloCelda));
                    _cell = new Cell().Add(new Paragraph("Especie"));
                    _table.AddHeaderCell(_cell.AddStyle(styloCelda));
                    _cell = new Cell().Add(new Paragraph("Raza"));
                    _table.AddHeaderCell(_cell.AddStyle(styloCelda));
                    _cell = new Cell().Add(new Paragraph("Estado"));
                    _table.AddHeaderCell(_cell.AddStyle(styloCelda));

                    //información dentro de la base de datos
                    List<Mascota> model = db.Mascota.ToList();

                    int x = 0;
                    foreach (var item in model)
                    {
                        x++;
                        _cell = new Cell().Add(new Paragraph(x.ToString()));
                        _table.AddCell(_cell);
                        _cell = new Cell().Add(new Paragraph(item.nombre_mascota));
                        _table.AddCell(_cell);
                        _cell = new Cell().Add(new Paragraph(item.Especie.nombre));
                        _table.AddCell(_cell);
                        _cell = new Cell().Add(new Paragraph(item.Raza.nombre));
                        _table.AddCell(_cell);
                        _cell = new Cell().Add(new Paragraph(item.Estado_mascota.nombre_estado));
                        _table.AddCell(_cell);
                    }
                }
            }else if (url[0] == "razas")
            {
                _table = new Table(3).UseAllAvailableWidth();//cambiamos la cantidad de columnas a tener en el pdf
                Cell _cell = new Cell().Add(new Paragraph("#"));
                _table.AddHeaderCell(_cell.AddStyle(styloCelda));
                _cell = new Cell().Add(new Paragraph("Nombre de Raza"));
                _table.AddHeaderCell(_cell.AddStyle(styloCelda));
                _cell = new Cell().Add(new Paragraph("Especie"));
                _table.AddHeaderCell(_cell.AddStyle(styloCelda));

                //información dentro de la base de datos
                List<Raza> model = db.Raza.ToList();

                int x = 0;
                foreach (var item in model)
                {
                    x++;
                    _cell = new Cell().Add(new Paragraph(x.ToString()));
                    _table.AddCell(_cell);
                    _cell = new Cell().Add(new Paragraph(item.nombre));
                    _table.AddCell(_cell);
                    _cell = new Cell().Add(new Paragraph(item.Especie.nombre));
                    _table.AddCell(_cell);
                }
            }
            else if (url[0] == "vacunas")
            {
                _table = new Table(3).UseAllAvailableWidth();//cambiamos la cantidad de columnas a tener en el pdf
                Cell _cell = new Cell().Add(new Paragraph("#"));
                _table.AddHeaderCell(_cell.AddStyle(styloCelda));
                _cell = new Cell().Add(new Paragraph("Nombre de la Vacuna"));
                _table.AddHeaderCell(_cell.AddStyle(styloCelda));
                _cell = new Cell().Add(new Paragraph("Descripción"));
                _table.AddHeaderCell(_cell.AddStyle(styloCelda));

                //información dentro de la base de datos
                List<Vacunas> model = db.Vacunas.ToList();

                int x = 0;
                foreach (var item in model)
                {
                    x++;
                    _cell = new Cell().Add(new Paragraph(x.ToString()));
                    _table.AddCell(_cell);
                    _cell = new Cell().Add(new Paragraph(item.vacuna));
                    _table.AddCell(_cell);
                    _cell = new Cell().Add(new Paragraph(item.descripcion));
                    _table.AddCell(_cell);
                }
            }
            else
            {
                Cell _cell = new Cell().Add(new Paragraph("Nuevo PDF"));
                _table.AddHeaderCell(_cell.AddStyle(styloCelda));
            }


            //agregamos la 2a tabla
            doc.Add(_table);


            // Cerrando el documento
            doc.Close();

            byte[] bytesStream = ms.ToArray();
            ms = new MemoryStream();
            ms.Write(bytesStream,0,bytesStream.Length);
            ms.Position = 0;

            return new FileStreamResult(ms, "application/pdf");
        }
    }


    //por medio de eventos sabremos cuando agregar texto e imágenes al footer o al header
    public class HeaderEventHandler1 : IEventHandler
    {
        Image Img;
        Document doc;
        public HeaderEventHandler1(Image img)
        {
            Img = img;
        }
        public HeaderEventHandler1(Image img, Document _doc)
        {
            Img = img;
            this.doc = _doc;
        }

        public void HandleEvent(Event @event)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
            PdfDocument pdfDoc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();

            float mSup = doc.GetTopMargin();
            float mInf = doc.GetBottomMargin();
            float mIzq = doc.GetLeftMargin();
            float mDer = doc.GetRightMargin();


            //para ello creamos un rectángulo para dimensionar el tamaño de donde queremos el contenido
            //Rectangle rootArea = new Rectangle(35, page.GetPageSize().GetTop() - 75, page.GetPageSize().GetWidth() - 70, 55);
            Rectangle rootArea = new Rectangle(mIzq, page.GetPageSize().GetTop() - mSup, page.GetPageSize().GetWidth() - (mIzq + mDer), (mSup+mIzq)/2);

            //agregamos el contenido antes de que se cree la siguiente página
            //para ello usamos StreamBefore(), luego que re cursos usar y en que documento
            PdfCanvas canvas1 = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
            new Canvas(canvas1, pdfDoc, rootArea).Add(getTable(docEvent));

        }

        public Table getTable(PdfDocumentEvent docEvent)
        {
            float[] cellWidth = { 20f, 80f };
            Table tableEvent = new Table(UnitValue.CreatePercentArray(cellWidth)).UseAllAvailableWidth();

            //estilos a usar 
            Style styleCell = new Style().SetBorder(Border.NO_BORDER);
            Style styleText = new Style().SetTextAlignment(TextAlignment.RIGHT).SetFontSize(10f);

            //creamos la 1er celda que contendrá la imagen 
            Cell cell = new Cell().Add(Img.SetAutoScale(true));

            tableEvent.AddCell(cell
                .AddStyle(styleCell)
                .SetTextAlignment(TextAlignment.LEFT));

            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
            
            //dibujamos el header
            cell = new Cell()
                .Add(new Paragraph("Reporte diario\n").SetFont(bold))
                .Add(new Paragraph("Save Our Pets\n").SetFont(bold))
                .Add(new Paragraph("Fecha de emisión: " + DateTime.Now.ToShortDateString()))
                .AddStyle(styleText).AddStyle(styleCell);

            tableEvent.AddCell(cell);

            return tableEvent;
        }
    }

    public class FooterEventHandler1 : IEventHandler
    {
        
        Document doc;
        public FooterEventHandler1(Document _doc)
        {
            this.doc = _doc;
        }


        public void HandleEvent(Event @event)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
            PdfDocument pdfDoc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();

            float mSup = doc.GetTopMargin();
            float mInf = doc.GetBottomMargin();
            float mIzq = doc.GetLeftMargin();
            float mDer = doc.GetRightMargin();



            //para ello creamos un rectángulo para dimensionar el tamaño de donde queremos el contenido
            //Rectangle rootArea = new Rectangle(36, 20, page.GetPageSize().GetWidth() - 70, 50);
            Rectangle rootArea = new Rectangle(36, 20, page.GetPageSize().GetWidth() - (mDer+mIzq), mInf-20);

            //agregamos el contenido antes de que se cree la siguiente página
            //para ello usamos StreamBefore(), luego que re cursos usar y en que documento
            PdfCanvas canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
            new Canvas(canvas, pdfDoc, rootArea).Add(getTable(docEvent));
        }

        public Table getTable(PdfDocumentEvent docEvent)
        {
            float[] cellWidth = { 92f, 8f };
            Table tableEvent = new Table(UnitValue.CreatePercentArray(cellWidth)).UseAllAvailableWidth();

            //obtenemos el número de página
            int pageNum = docEvent.GetDocument().GetPageNumber(docEvent.GetPage());

            //estilo a usar 
            Style styleCell = new Style()
                .SetPadding(5)
                .SetBorder(Border.NO_BORDER)
                .SetBorderTop(new SolidBorder(ColorConstants.BLACK, 2));

            Cell cell = new Cell().Add(new Paragraph(DateTime.Now.ToLongDateString()));
            tableEvent.AddCell(cell.AddStyle(styleCell)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontColor(ColorConstants.LIGHT_GRAY));

            cell = new Cell().Add(new Paragraph(pageNum.ToString()));
            cell.AddStyle(styleCell)
                .SetBackgroundColor(ColorConstants.BLACK)
                .SetFontColor(ColorConstants.WHITE)
                .SetTextAlignment(TextAlignment.CENTER);
            tableEvent.AddCell(cell);

            return tableEvent;
        }
    }
}
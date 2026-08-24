using AutoMapper;
using BackendAPI3._1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAIH_Backend.Datos.Entidades;
using SAIH_Backend.Servicios.Clases_estaticas;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApiSAIH.Models;
using WebApiSAIH.Services.DTO;
using WebApiSAIH.Services.Interfaces;
using WebApiSAIH.Services.VOs;

namespace WebApiSAIH.Services.Implementacion
{
    public class ServicioDocument : IServicioDocument
    {
        private readonly string AppDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ServicioDocument(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RespuestaGenerica> descargarDocument(long id)
        {
            try
            {
                if (!Directory.Exists(AppDirectory))
                    Directory.CreateDirectory(AppDirectory);

                var file = _context.Documents.Where(n => n.DocumentId == id).FirstOrDefault();

                if (file == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Document no encontrado", null);
                }

                var path = Path.Combine(AppDirectory, file.FilePath);

                if (!File.Exists(path))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Document no encontrado", null);
                }

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                var contentType = "APPLICATION/octet-stream";
                var fileName = Path.GetFileName(path);

                DatosDocument datosDocument = new DatosDocument {
                    Memory = memory,
                    ContentType =  contentType,
                    FileName =  fileName
                };

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Document descargandose...", datosDocument);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }
        }

        public RespuestaGenerica eliminarDocument(long id)
        {
            try
            {
                Document file = _context.Documents.Where(n => n.DocumentId == id).FirstOrDefault();

                if (file != null)
                {
                    _context.Remove(file);
                    _context.SaveChanges();

                    var folderName = AppDirectory;
                    var pathToDelete = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var fullPath = Path.Combine(pathToDelete, file.Name);

                    File.Delete(fullPath);

                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Document eliminado", file.Name);
                }
                else
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Document no encontrado", null);
                }

            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error interno del servidor", e.Message);
            }
        }

        public RespuestaGenerica guardarDocument(FormFile files)
        {
            try
            {
                var file = files;
                var folderName = AppDirectory;
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    if (!Directory.Exists(AppDirectory))
                        Directory.CreateDirectory(AppDirectory);

                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    var fileName2 = file.FileName;
                    var path = Path.Combine(AppDirectory, fileName2);
                    var fileExtension = Path.GetExtension(fileName2);
                    var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

                    Document objFiles = new Document()
                    {
                        Name = fileName2,
                        FilePath = path,
                        FileType = fileExtension,
                        CreatedOn = DateTime.Now
                    };

                    using (var target = new MemoryStream())
                    {
                        file.CopyTo(target);
                        objFiles.DataFiles = target.ToArray();
                    }

                    _context.Documents.Add(objFiles);
                    _context.SaveChanges();

                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK,"Document guardado", objFiles);
                }
                else
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_BAD_REQUEST, "BAD REQUEST", null);
                }
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error" , e.Message);
            }
        }

        public RespuestaGenerica obtenerNombrePkDocument(long pk_idDocument)
        {
            try
            {
                Document document = _context.Documents.
                    Where(s => s.DocumentId == pk_idDocument).FirstOrDefault<Document>();

                if (document == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_BAD_REQUEST, "No hay document con esa pk", null);
                }

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Nombre del document", document.Name);

            }
            catch(Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }
        }

        public RespuestaGenerica obtenerDocuments()
        {
            List<Document> documents = _context.Documents.Select(n => new Document
            {
                DocumentId = n.DocumentId,
                Name = n.Name,
                FileType = n.FileType,
                FilePath = n.FilePath,
                CreatedOn = n.CreatedOn
            }).ToList();

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Documents desplegados", _mapper.Map<List<DocumentDTO>>(documents));
        }

        public RespuestaGenerica obtenerDocumentXemployee(string cedula)
        {
            try
            {
                Employee employee = _context.Employees
                    .Where(s => s.NationalId == cedula).FirstOrDefault<Employee>();

                if (employee == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No se encontro el employee", null);
                }

                Document document = _context.Documents.
                    Where(s => s.DocumentId == employee.FK_idDocument1).FirstOrDefault<Document>();

                if(document == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_BAD_REQUEST, "No hay imagen employee a este employee " + cedula, null);
                }

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Imagen asociada al employee " + cedula, document);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }
        }

        public RespuestaGenerica obtenerDocumentXemployeeBytes(string cedula)
        {
            try
            {
                Employee employee = _context.Employees
                    .Where(s => s.NationalId == cedula).FirstOrDefault<Employee>();

                if (employee == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No se encontro el employee", null);
                }

                Document document = _context.Documents.
                    Where(s => s.DocumentId == employee.FK_idDocument1).FirstOrDefault<Document>();

                if (document == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_BAD_REQUEST, "No hay imagen employee a este employee " + cedula, null);
                }

                var byteArrImg = File.ReadAllBytes(document.FilePath);
                var base64Img = Convert.ToBase64String(byteArrImg);

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Imagen asociada al employee " + cedula, base64Img);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }
        }

        public RespuestaGenerica obtenerImagenEmployee(string nationalId)
        {
            try
            {
                Employee employee = _context.Employees
                    .Where(s => s.NationalId == nationalId).FirstOrDefault<Employee>();

                if (employee == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No se encontro el employee", null);
                }

                Document document = _context.Documents.
                    Where(s => s.DocumentId == employee.FK_idDocument1).FirstOrDefault<Document>();

                string filePath = document.FilePath;
                string replaceCharacters = "";

                for(int i = 0; i <= 1; i++)
                {
                    replaceCharacters += filePath[i];
                }

                filePath = Regex.Replace(filePath, replaceCharacters, "http://127.0.0.1:8887");

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Ruta de la imagen: ", filePath);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }
        }
    }
}

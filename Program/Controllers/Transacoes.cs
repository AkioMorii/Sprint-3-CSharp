using AgendaContatos.Conexao;
using AgendaContatos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgendaContatos.Controllers
{
    public class TransacoesController : Controller
    {
        private readonly DataBaseConexao _context;

        public TransacoesController(DataBaseConexao context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string filtroNome)
        {
            var model = from t in _context.Transacoes select t;

            if (!string.IsNullOrEmpty(filtroNome))
            {
                model = model.Where(t => t.Nome.Contains(filtroNome));
            }
            return View(await model.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            var contatos = from c in _context.Contatos select c;
            ViewBag.Contatos = await contatos.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Transacoes transacoes)
        {
            if (ModelState.IsValid)
            {
                //Retirando mascará JS
                _context.Transacoes.Add(transacoes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transacoes);
        }
    }
}
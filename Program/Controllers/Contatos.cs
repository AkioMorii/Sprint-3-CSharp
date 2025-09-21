using AgendaContatos.Conexao;
using AgendaContatos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgendaContatos.Controllers
{
    public class ContatosController : Controller
    {
        private readonly DataBaseConexao _context;

        public ContatosController(DataBaseConexao context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string filtroNome)
        {
            var contatos = from c in _context.Contatos select c;

            if (!string.IsNullOrEmpty(filtroNome))
            {
                contatos = contatos.Where(c => c.Nome.Contains(filtroNome));
            }

            return View(await contatos.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contatos contato)
        {
            if (ModelState.IsValid)
            {
                //Retirando mascará JS
                contato.CPF = contato.CPF.Replace(".", "").Replace("-", "");
                contato.Telefone = contato.Telefone.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");

                contato.DataCadastro = DateTime.UtcNow;
                contato.UltimaAlteracao = DateTime.UtcNow;
                _context.Contatos.Add(contato);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contato);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var contato = await _context.Contatos.FindAsync(id);
            if (contato == null) return NotFound();

            return View(contato);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contatos contato)
        {
            if (id != contato.Id) return NotFound();

            if (ModelState.IsValid)
            {
                //Retirando mascará JS
                contato.CPF = contato.CPF.Replace(".", "").Replace("-", "");
                contato.Telefone = contato.Telefone.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");

                try
                {
                    contato.UltimaAlteracao = DateTime.UtcNow;
                    _context.Update(contato);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Contatos.Any(e => e.Id == contato.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contato);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var contato = await _context.Contatos.FirstOrDefaultAsync(m => m.Id == id);
            if (contato == null) return NotFound();

            return View(contato);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contato = await _context.Contatos.FindAsync(id);
            _context.Contatos.Remove(contato);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var contato = await _context.Contatos.FirstOrDefaultAsync(m => m.Id == id);
            if (contato == null) return NotFound();
            else return View(contato);
        }
    }
}

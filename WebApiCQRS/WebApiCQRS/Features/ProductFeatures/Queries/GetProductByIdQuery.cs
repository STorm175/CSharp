using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class GetProductByIdQuery : IRequest<Product>
{
    public int Id { get; set; }
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly IApplicationContext _context;
        public GetProductByIdQueryHandler(IApplicationContext context)
        {
            _context = context;
        }
        public async Task<Product> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = _context.Products.Where(a => a.Id == query.Id).FirstOrDefault();
            if (product == null) return null;
            return product;
        }
    }
}
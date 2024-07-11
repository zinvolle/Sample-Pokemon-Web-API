using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using webAPI2.DTO;
using webAPI2.Interface;
using webAPI2.Models;

namespace webAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepositoy, IReviewerRepository reviewerRepository, IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepositoy;
            _reviewerRepository = reviewerRepository;
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;

        }

        [HttpGet]
        public IActionResult GetReviews()
        {
            var reviews = _reviewRepository.GetReviews();
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }

        [HttpGet("{reviewId}")]
        public IActionResult GetReview(int reviewId)
        {
            var review = _reviewRepository.GetReview(reviewId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(review);
        }

        [HttpGet("pokemon/{pokeId}")]
        public IActionResult GetReviewsOfPokemon(int pokeId)
        {
            var pokeReviews = _reviewRepository.GetReviewsOfPokemon(pokeId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokeReviews);
        }

        [HttpPost]
        public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokeId, [FromBody] ReviewDto reviewCreate)
        {
            if (reviewCreate == null)
            {
                return BadRequest(ModelState);
            }
            var review = _reviewRepository.GetReviews().Where(
                r => r.Title.Trim().ToUpper() == reviewCreate.Title.TrimEnd().ToUpper()).FirstOrDefault();
            if (review != null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }
            var reviewMap = _mapper.Map<Review>(reviewCreate);
            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokeId);
            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);

            if (!_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "There was a problem while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
    }
}

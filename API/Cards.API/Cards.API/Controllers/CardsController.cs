﻿using Cards.API.Data;
using Cards.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cards.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CardsController : Controller
    {

        private readonly CardsDbContext cardsDbContext;
        public CardsController(CardsDbContext cardsDbContext)
        {
            this.cardsDbContext = cardsDbContext;
        }


        //Get All Cards
        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var cards = await cardsDbContext.Cards.ToListAsync();
            return Ok(cards);
        }

        //Get By ID
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetCard")]
        public async Task<IActionResult> GetCard([FromRoute] Guid id)
        {
            var card=await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if(card != null)
            {
                return Ok(card);
            }
            return NotFound("Card Not Found");
        }

        [HttpPost]
        public async Task<IActionResult> AddCard([FromBody] Card card)
        {
            card.Id = Guid.NewGuid();

            await cardsDbContext.Cards.AddAsync(card);

            await cardsDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCard),new { id=card.Id}, card);

            
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCard([FromRoute] Guid id, [FromBody] Card card)
        {
            var existingCard=await cardsDbContext.Cards.FirstOrDefaultAsync(x=>x.Id == id);
            if(card != null)
            {
                existingCard.CardHolderName = card.CardHolderName;
                existingCard.CardNumber = card.CardNumber;
                existingCard.ExpiryMonth = card.ExpiryMonth;
                existingCard.ExpiryYear = card.ExpiryYear;
                existingCard.CVC=card.CVC;
                await cardsDbContext.SaveChangesAsync(); 
                return Ok(existingCard);
            }
            return NotFound("Card Not found");
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [ActionName("GetCard")]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        {
            var card = await cardsDbContext.Cards.FirstOrDefaultAsync();
            if (card!=null)
            {
                cardsDbContext.Remove(card);
                cardsDbContext.SaveChanges();
                return Ok(card);
                
            }
            return NotFound("Card Not Found");
        }


    }
}

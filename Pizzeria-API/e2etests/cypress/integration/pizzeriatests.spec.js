/// <reference types="cypress" />

describe('Pizzeria tests', () => {
    const frontendUrl = 'http://localhost:80';
    const storageUrl = 'http://localhost:6001/api/orders/';
    let orderId = 0;
    let ingredientQuantityOnPizza = 0;
    let ingredientQuantityBeforeOrder = 0;
    let initialIngredientQuantity = 0;

    it('Get initial ingredient quantity of ham in the storage', () => {
        cy.visit(frontendUrl);
        cy.request('http://localhost:6002/api/IngredientItems/ham')
            .then((response) => {
                initialIngredientQuantity = response.body;
            });
            
    });

    it('Add ham to the storage', () => {
        cy.visit(frontendUrl);
        ingredientQuantityBeforeOrder = 10;
        cy.get('#updated-quantity1')
            .clear()
            .type(`${ingredientQuantityBeforeOrder}`);
        cy.get('#updated-quantity-button1')
            .click();
        cy.get('#updated-quantity1')
            .invoke('val')
            .then(parseInt)
            .should('be.eq', ingredientQuantityBeforeOrder);

    });

    it('Create order with a pizza', () => {
        const pizzaName = 'Margherita';
        cy.request('POST', storageUrl + `${pizzaName}`)
            .then((response) => {
                expect(response.status).to.eq(200);
                expect(response.body.pizzas[0].name).to.eq(pizzaName);
                orderId = response.body.id;
            })
    });

    it('Add ingredients to pizza', () => {
        const ingredientName = 'Ham';
        ingredientQuantityOnPizza = 2;
        var i;
        for (i = 0; i < ingredientQuantityOnPizza; i++) {
            cy.request('PUT', storageUrl + `${orderId}/add=${ingredientName}`)
                .then((response) => {
                    expect(response.status).to.eq(200);
                    expect(response.body.ingredients[0].name).to.eq(ingredientName);
                })
        }
               
    });

    it('Submit order for a pizza with extra ingredients', () => {
        const statusForSubmitedOrder = 1;
        cy.request('PUT', storageUrl + `${orderId}/submit`)
            .then((response) => {
                expect(response.status).to.eq(200);
                expect(response.body.status).to.eq(statusForSubmitedOrder);
            })
    });

    it('Check ingredient quantity after the order was submitted', () => {
        cy.visit(frontendUrl);
        let hamQuantityAfterOrder = ingredientQuantityBeforeOrder - ingredientQuantityOnPizza;
        cy.get('#updated-quantity1')
            .invoke('val')
            .then(parseFloat)
            .should('be.eq', hamQuantityAfterOrder);

    });

    it('Restore ingredient quantity as before executing e2e tests', () => {
        cy.visit(frontendUrl);

        cy.get('#updated-quantity1')
            .clear()
            .type(`${initialIngredientQuantity}`);
        cy.get('#updated-quantity-button1')
            .click();
        cy.get('#updated-quantity1')
            .invoke('val')
            .then(parseInt)
            .should('be.eq', initialIngredientQuantity);
    });
     
})


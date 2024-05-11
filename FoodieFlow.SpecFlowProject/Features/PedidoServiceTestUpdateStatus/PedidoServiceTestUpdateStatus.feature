Feature: Controle de pedidos
    As a user
    I want to manage pedidos
    So that I can keep track of all pedidos

 Scenario: Update pedido status
        Given I have a Pedido to update
        When I call UpdateStatus with id '1' and status 'pago'
        Then the status of pedido with id '1' should be updated to 'pronto'



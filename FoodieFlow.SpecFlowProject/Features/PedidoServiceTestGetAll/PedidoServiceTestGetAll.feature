Feature: Controle de pedidos
    As a user
    I want to manage pedidos
    So that I can keep track of all pedidos

    Scenario: Get all pedidos
        Given I have a kitchen and have pedidos
        When I call GetAll with status 'a_pagar'
        Then I should get a list of all pedidos with status 'a_pagar'

   
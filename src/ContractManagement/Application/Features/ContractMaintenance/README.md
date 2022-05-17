# ContractMaintenance Feature

For this feature we chose an alternative way of keeping the code for a certain feature together. In stead of creating separate classes for the *CommandHandler*, *EventHandler* and *Projection*, we place the code in a single class (named after the feature) that implements all the necessary interfaces: `ICommandHandler`, `IEventHandler` and `IProjection`. This approach is possible because these responsibilities are strictly separated in the Onion architecture approach. Therefore, it's up to the development team to choose an approach that suits them.

Because we use Scrutor for automatically registering services for dependency injection, changing the approach can be done without changes to the registration code.

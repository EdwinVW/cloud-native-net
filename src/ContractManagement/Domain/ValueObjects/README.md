# DDD Value-objects

- We use value-objects only within the domain.
- We never use them in events and commands (only primitive types).
- We implement invariants in the ValueObject using a `Parse` method.
- Within the domain we ALWAYS use the `Parse` method to create Value-object instances.
- A Value-object constructor does NEVER check invariants. This constructor is only used when rehydrating aggregates from storage and the value should always be considered valid because it went through the invariant checks when it was created. This allows for the invariants to change over time without breaking rehydration of aggregates from storage. You might need to implement some conversion logic in the constructor when invariants change. This is considered per value-object.
- We don't implement implicit cast operators to prevent excessive boxing/unboxing.

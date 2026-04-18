Feature: ActivityQueryService

    Background:
        Given the activity database is empty

    Scenario: GetGlobalActivity returns all entries ordered by timestamp descending
        Given the following activity entries exist:
            | Actor              | ResourceType | ResourceId | Action | Timestamp            |
            | officer@police.uk  | Submission   | sub-001    | 1      | 2026-01-01T10:00:00Z |
            | officer@police.uk  | Submission   | sub-002    | 2      | 2026-01-03T10:00:00Z |
            | officer@police.uk  | Submission   | sub-003    | 1      | 2026-01-02T10:00:00Z |
        When GetGlobalActivityAsync is called with take 100
        Then 3 entries are returned
        And the first entry has resource id "sub-002"
        And the last entry has resource id "sub-001"

    Scenario: GetGlobalActivity respects the take limit
        Given the following activity entries exist:
            | Actor             | ResourceType | ResourceId | Action | Timestamp            |
            | officer@police.uk | Submission   | sub-001    | 1      | 2026-01-01T10:00:00Z |
            | officer@police.uk | Submission   | sub-002    | 1      | 2026-01-02T10:00:00Z |
            | officer@police.uk | Submission   | sub-003    | 1      | 2026-01-03T10:00:00Z |
        When GetGlobalActivityAsync is called with take 2
        Then 2 entries are returned

    Scenario: GetGlobalActivity returns empty when no entries exist
        When GetGlobalActivityAsync is called with take 100
        Then 0 entries are returned

    Scenario: GetActivityForResource returns only matching entries
        Given the following activity entries exist:
            | Actor             | ResourceType | ResourceId | Action | Timestamp            |
            | officer@police.uk | Submission   | sub-001    | 1      | 2026-01-01T10:00:00Z |
            | officer@police.uk | Submission   | sub-002    | 1      | 2026-01-02T10:00:00Z |
            | officer@police.uk | Submission   | sub-001    | 2      | 2026-01-03T10:00:00Z |
        When GetActivityForResourceAsync is called for resource type "Submission" and id "sub-001" with take 100
        Then 2 entries are returned
        And all entries have resource id "sub-001"

    Scenario: GetActivityForResource returns empty when no matching entries exist
        Given the following activity entries exist:
            | Actor             | ResourceType | ResourceId | Action | Timestamp            |
            | officer@police.uk | Submission   | sub-001    | 1      | 2026-01-01T10:00:00Z |
        When GetActivityForResourceAsync is called for resource type "Submission" and id "sub-999" with take 100
        Then 0 entries are returned


Feature: ActivityChannelProcessor

    Scenario: Entry written to the channel is persisted to the activity database
        Given an ActivityLogEntry for resource "sub-001"
        When the entry is written to the channel and the processor runs
        Then 1 activity log entry exists for resource "sub-001"

    Scenario: Multiple entries written to the channel are all persisted
        Given 3 ActivityLogEntries for resource "sub-batch"
        When all entries are written to the channel and the processor runs
        Then 3 activity log entries exist for resource "sub-batch"


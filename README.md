## States Language

This library contains some helper classes to help creating and modifying workflow using the [Amazon States Language](https://states-language.net/spec.html).
This is the workflow description language used by [AWS StepFunctions](https://aws.amazon.com/step-functions)

This project starts as a port of the java livrary [light-workflow-4j project](https://github.com/networknt/light-workflow-4j).

 
# StateMachine Builder

```csharp
StateMachine stateMachine = StateMachineBuilder.StateMachine()
    .StartAt("InitialState")
    .TimeoutSeconds(30)
    .Comment("My Simple State Machine")
    .State("InitialState", StateMachineBuilder.SucceedState()
        .Comment("Initial State")
        .InputPath("$.input")
        .OutputPath("$.output"))
    .Build();

string json = stateMachine.ToJson();

var builder = StateMachine.FromJson(json);
```

# Examples

## Basic State Types

### Pass State

Pass states pass input to output, with optional data transformation:

```csharp
StateMachine passExample = StateMachineBuilder.StateMachine()
    .Comment("Pass state example")
    .StartAt("PassStateExample")
    .State("PassStateExample", StateMachineBuilder.PassState()
        .Comment("Processes the input and adds some fixed data")
        .InputPath("$.user")
        .ResultPath("$.userDetails")
        .Result(new JObject { ["role"] = "admin", ["access"] = "full" })
        .OutputPath("$")
        .Next("NextState"))
    .State("NextState", StateMachineBuilder.SucceedState())
    .Build();
```

### Task State

Task states represent a work unit in your workflow:

```csharp
StateMachine taskExample = StateMachineBuilder.StateMachine()
    .Comment("Task state example")
    .StartAt("TaskStateExample")
    .State("TaskStateExample", StateMachineBuilder.TaskState()
        .Comment("Executes a Lambda function")
        .Resource("arn:aws:lambda:us-east-1:123456789012:function:ProcessData")
        .InputPath("$.data")
        .ResultPath("$.taskResult")
        .TimeoutSeconds(30)
        .HeartbeatSeconds(10)
        .Retry(StateMachineBuilder.Retrier()
            .ErrorEquals("States.Timeout", "States.TaskFailed")
            .IntervalSeconds(3)
            .MaxAttempts(2)
            .BackoffRate(1.5))
        .Catch(StateMachineBuilder.Catcher()
            .ErrorEquals("States.ALL")
            .ResultPath("$.error")
            .Next("ErrorHandlingState"))
        .Next("SuccessState"))
    .State("ErrorHandlingState", StateMachineBuilder.SucceedState())
    .State("SuccessState", StateMachineBuilder.SucceedState())
    .Build();
```

### Choice State

Choice states add conditional branching to your workflow:

```csharp
StateMachine choiceExample = StateMachineBuilder.StateMachine()
    .Comment("Choice state example")
    .StartAt("ChoiceStateExample")
    .State("ChoiceStateExample", StateMachineBuilder.ChoiceState()
        .Comment("Directs the flow based on the input data")
        .InputPath("$.request")
        .Default("DefaultState")
        .Choice(StateMachineBuilder.Choice()
            .Condition(StateMachineBuilder.StringEquals("$.type", "express"))
            .Next("ExpressProcessingState"))
        .Choice(StateMachineBuilder.Choice()
            .Condition(StateMachineBuilder.NumericGreaterThan("$.value", 1000))
            .Next("HighValueProcessingState")))
    .State("DefaultState", StateMachineBuilder.SucceedState())
    .State("ExpressProcessingState", StateMachineBuilder.SucceedState())
    .State("HighValueProcessingState", StateMachineBuilder.SucceedState())
    .Build();
```

### Wait State

Wait states pause execution for a specified time:

```csharp
StateMachine waitExample = StateMachineBuilder.StateMachine()
    .Comment("Wait state example")
    .StartAt("WaitStateExample")
    .State("WaitStateExample", StateMachineBuilder.WaitState()
        .Comment("Waits for 10 seconds before proceeding")
        .Seconds(10)
        .Next("AfterWaitState"))
    .State("AfterWaitState", StateMachineBuilder.SucceedState())
    .Build();
```

### Parallel State

Parallel states execute multiple branches concurrently:

```csharp
StateMachine parallelExample = StateMachineBuilder.StateMachine()
    .Comment("Parallel state example")
    .StartAt("ParallelProcessing")
    .State("ParallelProcessing", StateMachineBuilder.ParallelState()
        .Comment("Process data in parallel branches")
        .Branch(StateMachineBuilder.SubStateMachine()
            .Comment("First branch - validation")
            .StartAt("Validate")
            .State("Validate", StateMachineBuilder.TaskState()
                .Resource("arn:aws:lambda:us-east-1:123456789012:function:ValidateData")
                .End(true)))
        .Branch(StateMachineBuilder.SubStateMachine()
            .Comment("Second branch - transformation")
            .StartAt("Transform")
            .State("Transform", StateMachineBuilder.TaskState()
                .Resource("arn:aws:lambda:us-east-1:123456789012:function:TransformData")
                .End(true)))
        .ResultPath("$.parallelResults")
        .Next("AfterParallel"))
    .State("AfterParallel", StateMachineBuilder.SucceedState())
    .Build();
```

### Map State

Map states execute the same steps for each item in an array:

```csharp
StateMachine mapExample = StateMachineBuilder.StateMachine()
    .Comment("Map state example")
    .StartAt("ProcessItems")
    .State("ProcessItems", StateMachineBuilder.MapState()
        .Comment("Process each item in the input array")
        .ItemProcessor(StateMachineBuilder.SubStateMachine()
            .StartAt("ProcessItem")
            .State("ProcessItem", StateMachineBuilder.TaskState()
                .Resource("arn:aws:lambda:us-east-1:123456789012:function:ProcessSingleItem")
                .End(true)))
        .MaxConcurrency(5)
        .ItemsPath("$.items")
        .ResultPath("$.processedItems")
        .Next("FinishProcessing"))
    .State("FinishProcessing", StateMachineBuilder.SucceedState())
    .Build();
```

## Complex Examples

### Order Processing Workflow

This example demonstrates a more complex order processing workflow:

```csharp
StateMachine orderProcessing = StateMachineBuilder.StateMachine()
    .Comment("Order processing workflow")
    .StartAt("ValidateOrder")
    .State("ValidateOrder", StateMachineBuilder.TaskState()
        .Resource("arn:aws:lambda:us-east-1:123456789012:function:ValidateOrder")
        .ResultPath("$.validation")
        .Next("CheckValidation"))
    .State("CheckValidation", StateMachineBuilder.ChoiceState()
        .Choice(StateMachineBuilder.Choice()
            .Condition(StateMachineBuilder.BooleanEquals("$.validation.isValid", true))
            .Next("ProcessPayment"))
        .Default("NotifyInvalidOrder"))
    .State("ProcessPayment", StateMachineBuilder.TaskState()
        .Resource("arn:aws:lambda:us-east-1:123456789012:function:ProcessPayment")
        .ResultPath("$.payment")
        .Next("CheckPayment"))
    .State("CheckPayment", StateMachineBuilder.ChoiceState()
        .Choice(StateMachineBuilder.Choice()
            .Condition(StateMachineBuilder.StringEquals("$.payment.status", "succeeded"))
            .Next("FulfillOrder"))
        .Default("NotifyPaymentFailure"))
    .State("FulfillOrder", StateMachineBuilder.ParallelState()
        .Branch(StateMachineBuilder.SubStateMachine()
            .StartAt("UpdateInventory")
            .State("UpdateInventory", StateMachineBuilder.TaskState()
                .Resource("arn:aws:lambda:us-east-1:123456789012:function:UpdateInventory")
                .End(true)))
        .Branch(StateMachineBuilder.SubStateMachine()
            .StartAt("ShipOrder")
            .State("ShipOrder", StateMachineBuilder.TaskState()
                .Resource("arn:aws:lambda:us-east-1:123456789012:function:ShipOrder")
                .End(true)))
        .ResultPath("$.fulfillment")
        .Next("NotifySuccess"))
    .State("NotifyInvalidOrder", StateMachineBuilder.TaskState()
        .Resource("arn:aws:lambda:us-east-1:123456789012:function:NotifyInvalidOrder")
        .Next("OrderFailed"))
    .State("NotifyPaymentFailure", StateMachineBuilder.TaskState()
        .Resource("arn:aws:lambda:us-east-1:123456789012:function:NotifyPaymentFailure")
        .Next("OrderFailed"))
    .State("NotifySuccess", StateMachineBuilder.TaskState()
        .Resource("arn:aws:lambda:us-east-1:123456789012:function:NotifySuccess")
        .Next("OrderSucceeded"))
    .State("OrderFailed", StateMachineBuilder.FailState()
        .Error("Order.Failed")
        .Cause("Order processing failed"))
    .State("OrderSucceeded", StateMachineBuilder.SucceedState())
    .Build();
```

### Data Transformation Pipeline

This example shows how to build a data transformation pipeline with error handling:

```csharp
StateMachine dataProcessingPipeline = StateMachineBuilder.StateMachine()
    .Comment("Data processing pipeline")
    .StartAt("ExtractData")
    .State("ExtractData", StateMachineBuilder.TaskState()
        .Resource("arn:aws:lambda:us-east-1:123456789012:function:ExtractData")
        .ResultPath("$.extractedData")
        .Retry(StateMachineBuilder.Retrier()
            .ErrorEquals("States.ALL")
            .MaxAttempts(3)
            .IntervalSeconds(1)
            .BackoffRate(2))
        .Catch(StateMachineBuilder.Catcher()
            .ErrorEquals("States.ALL")
            .Next("HandleExtractError"))
        .Next("ValidateSchema"))
    .State("ValidateSchema", StateMachineBuilder.TaskState()
        .Resource("arn:aws:lambda:us-east-1:123456789012:function:ValidateSchema")
        .ResultPath("$.validationResult")
        .Next("CheckValidation"))
    .State("CheckValidation", StateMachineBuilder.ChoiceState()
        .Choice(StateMachineBuilder.Choice()
            .Condition(StateMachineBuilder.BooleanEquals("$.validationResult.valid", true))
            .Next("TransformData"))
        .Default("HandleValidationError"))
    .State("TransformData", StateMachineBuilder.MapState()
        .ItemProcessor(StateMachineBuilder.SubStateMachine()
            .StartAt("TransformItem")
            .State("TransformItem", StateMachineBuilder.TaskState()
                .Resource("arn:aws:lambda:us-east-1:123456789012:function:TransformItem")
                .Catch(StateMachineBuilder.Catcher()
                    .ErrorEquals("TransformationError")
                    .ResultPath("$.transformError")
                    .Next("HandleItemError"))
                .Next("ValidateItem"))
            .State("ValidateItem", StateMachineBuilder.TaskState()
                .Resource("arn:aws:lambda:us-east-1:123456789012:function:ValidateItem")
                .End(true))
            .State("HandleItemError", StateMachineBuilder.TaskState()
                .Resource("arn:aws:lambda:us-east-1:123456789012:function:LogItemError")
                .End(true)))
        .ItemsPath("$.extractedData.items")
        .ResultPath("$.transformedData")
        .Next("AggregateResults"))
    .State("AggregateResults", StateMachineBuilder.TaskState()
        .Resource("arn:aws:lambda:us-east-1:123456789012:function:AggregateResults")
        .ResultPath("$.aggregatedData")
        .Next("StoreResults"))
    .State("StoreResults", StateMachineBuilder.TaskState()
        .Resource("arn:aws:lambda:us-east-1:123456789012:function:StoreResults")
        .ResultPath("$.storageDetails")
        .Next("NotifyCompletion"))
    .State("NotifyCompletion", StateMachineBuilder.TaskState()
        .Resource("arn:aws:lambda:us-east-1:123456789012:function:NotifyCompletion")
        .Next("PipelineSucceeded"))
    .State("HandleExtractError", StateMachineBuilder.TaskState()
        .Resource("arn:aws:lambda:us-east-1:123456789012:function:HandleExtractError")
        .Next("PipelineFailed"))
    .State("HandleValidationError", StateMachineBuilder.TaskState()
        .Resource("arn:aws:lambda:us-east-1:123456789012:function:HandleValidationError")
        .Next("PipelineFailed"))
    .State("PipelineSucceeded", StateMachineBuilder.SucceedState())
    .State("PipelineFailed", StateMachineBuilder.FailState()
        .Error("Pipeline.Failed")
        .Cause("Pipeline processing failed"))
    .Build();
```

### Complex Condition Example

This example demonstrates complex conditions with logical operators:

```csharp
StateMachine complexConditionsExample = StateMachineBuilder.StateMachine()
    .Comment("Complex conditions example")
    .StartAt("EvaluateConditions")
    .State("EvaluateConditions", StateMachineBuilder.ChoiceState()
        .Choice(StateMachineBuilder.Choice()
            .Condition(StateMachineBuilder.And(
                StateMachineBuilder.NumericGreaterThanEquals("$.value", 100),
                StateMachineBuilder.NumericLessThan("$.value", 1000),
                StateMachineBuilder.StringEquals("$.category", "electronics")))
            .Next("ProcessElectronics"))
        .Choice(StateMachineBuilder.Choice()
            .Condition(StateMachineBuilder.And(
                StateMachineBuilder.Not(StateMachineBuilder.StringEquals("$.region", "restricted")),
                StateMachineBuilder.Or(
                    StateMachineBuilder.StringEquals("$.priority", "high"),
                    StateMachineBuilder.NumericGreaterThan("$.value", 5000))))
            .Next("HighPriorityProcess"))
        .Choice(StateMachineBuilder.Choice()
            .Condition(StateMachineBuilder.And(
                StateMachineBuilder.IsPresent("$.specialHandling", true),
                StateMachineBuilder.Not(StateMachineBuilder.IsNull("$.handlingInstructions", true))))
            .Next("SpecialHandling"))
        .Default("StandardProcessing"))
    .State("ProcessElectronics", StateMachineBuilder.SucceedState())
    .State("HighPriorityProcess", StateMachineBuilder.SucceedState())
    .State("SpecialHandling", StateMachineBuilder.SucceedState())
    .State("StandardProcessing", StateMachineBuilder.SucceedState())
    .Build();
```

## Using IntrinsicFunctions in a State Machine

```csharp
// Create an intrinsic function registry with standard functions
var registry = new IntrinsicFunctionRegistry();
registry.RegisterStandardFunctions();

// Create a state machine with intrinsic functions
StateMachine intrinsicFunctionExample = StateMachineBuilder.StateMachine()
    .Comment("Intrinsic Functions Example")
    .StartAt("ProcessData")
    .State("ProcessData", StateMachineBuilder.PassState()
        .Parameters(new JObject
        {
            ["formattedData"] = new JObject
            {
                ["concat"] = new JArray { "Hello ", "$.name", "!" }
            },
            ["uppercaseName"] = new JObject
            {
                ["States.StringToUpper"] = "$.name"
            },
            ["timestamp"] = new JObject
            {
                ["States.Format"] = new JArray { "Current time: {}", "States.JsonToString(States.Now())" }
            },
            ["arraySize"] = new JObject
            {
                ["States.ArrayLength"] = "$.items"
            }
        })
        .ResultPath("$.processed")
        .Next("DisplayResults"))
    .State("DisplayResults", StateMachineBuilder.SucceedState())
    .Build();

// Process some input data
var processor = new InputOutputProcessor(registry);
var input = JObject.Parse(@"{
    ""name"": ""user"",
    ""items"": [1, 2, 3, 4, 5]
}");

// Get effective input with parameters processed by the intrinsic function registry
var effectiveInput = processor.GetEffectiveInput(
    input,
    new OptionalString("$"),
    ((JObject)intrinsicFunctionExample.States["ProcessData"].GetType().GetProperty("Parameters").GetValue(intrinsicFunctionExample.States["ProcessData"])),
    null);

// Result: effectiveInput will contain processed intrinsic functions
```

# Tools

## InputOutputProcessor 

```csharp
public interface IInputOutputProcessor
{
    JToken GetEffectiveInput(JToken input, OptionalString inputPath, JObject payload, JObject context);
    JToken GetEffectiveResult(JToken output, JObject payload, JObject context);
    JToken GetEffectiveOutput(JToken input, JToken result, OptionalString outputPath, OptionalString resultPath);
}
```

## IntrinsicFunctions support 

There is a IIntrinsicFunctionRegistry available to register your StateLanguage Intrinsic functions.

The Standard Intrinsic functions defined [here](https://states-language.net/spec.html#appendix-b) are already included in the registry.

```csharp
public interface IIntrinsicFunctionRegistry
{
    void Register(string name, IntrinsicFunctionFunc func);
    void Unregister(string name);
}

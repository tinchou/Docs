Filters
=======

By `Steve Smith`_

*Filters* in ASP.NET MVC allow you to run code before or after a particular stage in the execution pipeline. Filters can be configured globally, per-controller, or per-action.

.. contents:: Sections
    :local:
    :depth: 1

`View or download sample from GitHub <https://github.com/aspnet/Docs/tree/master/mvc/controllers/filters/sample>`_.

How do filters work?
--------------------

Each kind of filter will be executed at a different stage in the pipeline, and thus has its own set of intended scenarios. Choose what kind of filter to create based on the task you need it to perform, and where in the request pipeline it executes. Filters run within the MVC Action Invocation Pipeline, sometimes referred to as the *Filter Pipeline*, which runs after MVC selects the action to execute.

.. image:: filters/_static/filter-pipeline-1.png

Different kinds of filters run at different points within the pipeline. Some filters, like authorization filters, only run before the next stage in the pipeline, and take no action afterward. Other filters, like action filters, can execute both before and after other parts of the pipeline execute, as shown below.

.. image:: filters/_static/filter-pipeline-2.png

Which filter do I need?
^^^^^^^^^^^^^^^^^^^^^^^

:ref:`Authorization filters <authorization-filters>` are used to determine whether the current user is authorized for the request being made.

:ref:`Resource filters <resource-filters>` are the first filter to handle a request after authorization, and the last one to touch the request as it is leaving the filter pipeline. They're especially useful to implement caching or otherwise short-circuit the pipeline for performance reasons.

:ref:`Action filters <action-filters>` wrap calls to individual action method calls, and can manipulate the arguments passed into an action as well as the response returned from it.

:ref:`Exception filters <exception-filters>` are used to apply global policies to unhandled exceptions in the MVC app.

:ref:`Result filters <result-filters>` wrap the execution of individual action results, but only when no exceptions have been thrown. They are ideal for logic that must surround view execution or formatter execution.

Implementation
^^^^^^^^^^^^^^

All filters support both synchronous and asynchronous implementations through different interface definitions. Choose the sync or async variant depending on the kind of task you need to perform. They are interchangeable from the framework's perspective.

Synchonous filters define both an On\ *Stage*\ Executing and On\ *Stage*\ Executed method (with noted exceptions). The On\ *Stage*\ Executing method will be called before the event pipeline stage by the Stage name, and the On\ *Stage*\ Executed method will be called after the pipeline stage named by the Stage name.

.. literalinclude:: filters/sample/src/FiltersSample/Filters/SampleActionFilter.cs
  :language: c#
  :emphasize-lines: 6,8,13

Asynchronous filters define a single On\ *Stage*\ ExecutionAsync method that will surround execution of the pipeline stage named by Stage. The On\ *Stage*\ ExecutionAsync method is provided a *Stage*\ ExecutionDelegate delegate which will execute the pipeline stage named by Stage when invoked and awaited.

.. literalinclude:: filters/sample/src/FiltersSample/Filters/SampleAsyncActionFilter.cs
  :language: c#
  :emphasize-lines: 6,8-9

In most cases, you can implement both the async and synchronous interfaces with the same class, like so:

.. literalinclude:: filters/sample/src/FiltersSample/Filters/SampleCombinedActionFilter.cs
  :language: c#
  :emphasize-lines: 11-12

Filters and Attributes
^^^^^^^^^^^^^^^^^^^^^^
It's often convenient to implement filter interfaces as *Attributes*. These attributes are then used to apply the filter to certain controllers or action methods. The framework includes built-in attribute-based filters that you can subclass and customize. For example, the following filter inherits from `ResultFilterAttribute <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/Filters/ResultFilterAttribute/index.html>`_, and overrides its ``OnResultExecuting`` method to add a header to the response.

.. literalinclude:: filters/sample/src/FiltersSample/Filters/AddHeaderAttribute.cs
  :language: c#
  :emphasize-lines: 5,16

Attributes allow filters to accept arguments, as shown in the example above. You would add this attribute to a controller or action method and specify the name and value of the HTTP header you wished to be added to the response:

.. literalinclude:: filters/sample/src/FiltersSample/Controllers/SampleController.cs
  :language: c#
  :emphasize-lines: 6

The result is shown below - the response headers are displayed on the bottom right.

.. image:: filters/_static/add-header.png

The available built-in attribute-based filters are:
	- `ActionFilterAttribute <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/Filters/ActionFilterAttribute/index.html>`_
	- `AuthorizationFilterAttribute <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/Filters/AuthorizationFilterAttribute/index.html>`_
	- `ExceptionFilterAttribute <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/Filters/ExceptionFilterAttribute/index.html>`_
	- `ResultFilterAttribute <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/Filters/ResultFilterAttribute/index.html>`_

Dependency Injection
^^^^^^^^^^^^^^^^^^^^
Filters are not directly returned from :doc:`dependency injection </fundamentals/dependency-injection>` (DI). Filters that are also attributes, and which are added directly to controller classes or action methods cannot have constructor dependencies provided by DI - their constructor properties must be specified when they are declared. This is a limitation of how attributes work. However, if your filters have dependencies you need to access from DI, there are several supported approaches. You can apply your filter using either the `ServiceFilter <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/ServiceFilterAttribute/index.html>`_ or `TypeFilter <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/TypeFilterAttribute/index.html>`_ attribute, or you can implement IFilterFactory on your attribute, or you can access the services you require from the context parameter provided to your filter's On\ *Stage* methods.

Cancellation and Short Circuiting
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
You can short-circuit the filter pipeline at any point by setting the ``Result`` property on the context parameter provided to the filter method. For instance, the following ``ShortCircuitingResourceFilter`` will prevent any other filters from running later in the pipeline, including any action filters.

.. literalinclude:: filters/sample/src/FiltersSample/Filters/ShortCircuitingResourceFilter.cs
  :language: c#
  :emphasize-lines: 12-15

When this filter is applied to an action that also would have an action filter applied, such as the ``SomeResource`` action below, the action filter is not executed (in this case the action filter is applied at the class level, but the behavior would be the same if it were applied to the method).

.. literalinclude:: filters/sample/src/FiltersSample/Controllers/SampleController.cs
  :language: c#
  :emphasize-lines: 1,4
  :lines: 6-8, 14-19
  :dedent: 4 

Configuring Filters
-------------------
Filters can be *scoped* at three different levels. You can add a particular filter to a particular action as an attribute. You can add a filter to all actions within a controller by applying an attribute at the controller level. Or you can register a filter globally, to be run with every MVC action.

Filters are added globally in ``Startup``, when configuring MVC:

.. literalinclude:: filters/sample/src/FiltersSample/Startup.cs
  :language: c#
  :emphasize-lines: 3-7
  :lines: 15-23
  :dedent: 8
  :linenos:

Filters can be added by type, or an instance can be added. If you add an instance, that instance will be used for every request. If you add a type, the instance will be created through DI, and any constructor dependencies will be populated by DI. In the example above, the :ref:`DurationActionFilter <duration-action-filter>` has a dependency on ``ILoggerFactory`` in its constructor, which is fulfilled by DI on each request.

If you have a 
How, Where, Ordering, Scope Filters are not directly returned from DI You can write an attribute that has the filter interface on it or you can implement an attribute that implements IFilterFactory or you can use TypeFilter attribute or ServiceFilter attribute TypeFilter: news it up and passes params to its constructor from DI ServiceFilter: gets your filter from ServiceCollection (thus must be registered with DI) If you're using IFilterFactory, you can specify lifetime of the filter When your filter is the instance, your filter instance is cached, so don't do anything stateful. GlobalFilters are registered through MvcOptions
 
Ordering
^^^^^^^^
Filters can be applied per-action method (via attribute) or via controller (via attribute), or in global filters collection. Scope also generally determines ordering. The filter closest to the action runs first; generally you get overriding behavior without having to explicitly set ordering.

Yes, filters can implement IOrderedFilter to manually provide an ordering. See the docs here https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNetCore.Mvc.Abstractions/Filters/IOrderedFilter.cs for a precise, yet verbose description.

All of the built-in filters generally implement IOrderedFilter.

Let me also showcase that Order is a stronger qualifier than scope. Filters are sorted first by order, and then scope is used to break ties.

.. _authorization-filters:

Authorization Filters
---------------------
*Authorization Filters* control access to action methods, and are the first filters to be executed within the filter pipeline. They have only a before stage, unlike most filters that support before and after methods. You should only write a custom authorization filter if you are writing your own authorization framework. Note that you should not throw exceptions within authorization filters, since nothing will handle the exception (exception filters won't handle them). Instead, issue a challenge or find another way. They are covered in the :doc:`Security </security/index>` section of the documentation.

Learn more about :doc:`/security/authorization/authorization-filters`.

.. _resource-filters:

Resource Filters
----------------
*Resource Filters* implement either the ``IResourceFilter`` or ``IAsyncResourceFilter`` interface, and their execution wraps most of the filter pipeline (only :ref:`authorization filters` run before them).

.. _action-filters:

Action Filters
--------------
*Action Filters* implement either the ``IActionFilter`` or ``IAsyncActionFilter`` interface and their execution surrounds the execution of action methods. Action filters are ideal for any logic that needs to see the results of model binding, or modify the controller or inputs to an action method. Additionally, action filters can view and directly modify the result of an action method.

As the `OnActionExecuting` method runs before the action method, it can manipulate the inputs to the action by changing `ActionExecutingContext.ActionArguments` or manipulate the controller through `ActionExecutingContext.Controller`. An `OnActionExecuting` method can short-circuit execution of the action method and subsequent action filters by setting `ActionExecutingContext.Result`. Throwing an exception in an `OnActionExecuting` method will also prevent execution of the action method and subsequent filters, but will be treated as a failure instead of successful result.

The `OnActionExecuted` method runs after the action method, and can see and manipulate the results of the action through the `ActionExecutedContext.Result` property. `ActionExecutedContext.Canceled` will be set to true if the action execution was short-circuited by another filter. `ActionExecutedContext.Exception` will be set to a non-null value if the action or a subsequent action filter threw an exception. Setting `ActionExecutedContext.Exception` to null effectively 'handles' an exception, and `ActionExectedContext.Result` will be executed as if it were returned from the action method normally.

For an `IAsyncActionFilter` the `OnActionExecutionAsync` combines all the possibilites of `OnActionExecuting` and `OnActionExecuted`. A call to `await next()` on the `ActionExecutionDelegate` will execute any subsequent action filters and the action method, returning an `ActionExecutedContext`. To short-circuit inside of an `OnActionExecutionAsync`, set `ActionExecutingContext.Result` and do not call the `ActionExectionDelegate`.

.. _duration-action-filter:

SAMPLE

.. _exception-filters:

Exception Filters
-----------------
*Exception Filters* implement either the ``IExceptionFilter`` or ``IAsyncExceptionFilter`` interface.

Exception filters handle unhandled exceptions. They are only called when an exception occurs later in the pipeline. They can provide a single location to implement common error handling policies within an app. 

.. _result-filters:

Result Filters
--------------
*Result Filters* implement either the ``IResultFilter`` or ``IAsyncResultFilter`` interface and their execution surrounds the execution of action results. Result filters are only executed for successful results - when the action or action filters produces an action result. Result filters are not executed when exception filters handle an exception.

Result filters are ideal for any logic that needs to directly surround view execution or formatter execution. Result filters can replace or modify the action result that's responsible for producing the response.

As the `OnResultExecuting` method runs before the action action, it can manipulate the action result through `ResultExecutingContext.Result`. An `OnResultExecuting` method can short-circuit execution of the action result and subsequent result filters by setting `ResultExecutingContext.Cancel` to true. If short-circuited, MVC will not modify the response; take care to write to the response object directly when short-circuiting. Throwing an exception in an `OnResultExecuting` method will also prevent execution of the action result and subsequent filters, but will be treated as a failure instead of successful result.

The `OnResultExecuted` method runs after the action action, at this point if no exception was thrown, the response has likely be sent to the client and cannot be changed further. `ResultExecutedContext.Canceled` will be set to true if the action result execution was short-circuited by another filter. `ResultExecutedContext.Exception` will be set to a non-null value if the action result or a subsequent result filter threw an exception. Setting `ResultExecutedContext.Exception` to null effectively 'handles' an exception and will prevent the exeception from being rethrown by MVC later in the pipeline. If handling an exception in a result filter, consider whether or not it's appropriate to write any data to the response. The action result may have thrown partway through its execution, and if the headers have already been flushed to the client there's no proper recourse to send a failure status code. 

For an `IAsyncResultFilter` the `OnResultExecutionAsync` combines all the possibilites of `OnAResultExecuting` and `OnResultExecuted`. A call to `await next()` on the `ResultExecutionDelegate` will execute any subsequent result filters and the action result, returning a `ResultExecutedContext`. To short-circuit inside of an `OnResultExecutionAsync`, set `ResultExecutingContext.Cancel` to true and do not call the `ResultExectionDelegate`.

Filters vs. Middleware
----------------------
In general, filters are meant to handle cross-cutting business and application concerns. This is often the same use case for :doc:`middleware </fundamentals/middleware>`. Filters are very similar to middleware in capability, but let you scope that behavior and insert it into a location in your app where it makes sense, such as before a view, or after model binding. Filters are a part of MVC, and have access to its context and constructs. Middleware can't easily detect whether model validation on a request has generated errors, and respond accordingly, but a filter can easily do so.

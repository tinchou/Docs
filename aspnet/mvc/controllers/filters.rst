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

Each kind of filter will be executed at a different stage in the pipeline, and thus has its own set of intended scenarios. Choose what kind of filter to create based on the task you need it to perform, and where in the request pipeline it executes. Filters run within the MVC Action Invocation Pipeline, sometimes referred to as the *Filter Pipeline*, which runs after MVC selects the action to to execute.

.. image:: filters/_static/filter-pipeline-1.png

Different kinds of filters run at different points within the pipeline. Some filters, like authorization filters, only run before the next stage in the pipeline, and take no action afterward. Other filters, like action filters, can execute both before and after other parts of the pipeline execute, as shown below.

.. image:: filters/_static/filter-pipeline-2.png

All filters support both synchronous and asynchronous implementations through different interface definitions. Choose the sync or async variant depending on the kind of task you need to perform. They are interchangeable from the framework's perspective.

Synchonous filters define both an On*Stage*Executing and On*Stage*Executed method (with noted exceptions). The On*Stage*Executing method will be called before the event pipeline stage by the Stage name, and the On*Stage*Executed method will be called after the pipeline stage named by the Stage name.

EXAMPLE

Asynchronous filters define a single On*Stage*ExectionAsync method that will surround execution of the pipeline stage named by Stage. The On*Stage*ExectionAsync method is provided a *Stage*ExectionDelegate delegate which will execute the pipeline stage named by Stage when invoked and awaited.

EXAMPLE

Cancellation and Short Circuiting
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
asdf


Configuring Filters
-------------------
How, Where, Ordering, Scope Filters are not directly returned from DI You can write an attribute that has the filter interface on it or you can implement an attribute that implements IFilterFactory or you can use TypeFilter attribute or ServiceFilter attribute TypeFilter: news it up and passes params to its constructor from DI ServiceFilter: gets your filter from ServiceCollection (thus must be registered with DI) If you're using IFilterFactory, you can specify lifetime of the filter When your filter is the instance, your filter instance is cached, so don't do anything stateful. GlobalFilters are registered through MvcOptions
 
Scope
-----
Filter can be applied per-action method (via attribute) or via controller (via attribute), or in global filters collection. Scope also generally determines ordering. The filter closest to the action runs first; generally you get overriding behavior without having to explicitly set ordering.

Action Filters
--------------
*Action Filters implement either the ``IActionFilter`` or ``IAsyncActionFilter`` interface and their execution surrounds the execution of action methods. Action filters are ideal for any logic that needs to see the results of model binding, or modify the controller or inputs to an action method. Additionally, action filters can view and directly modify the result of an action method.

Result Filters
--------------

Exception Filters
-----------------

Authorization Filters
---------------------

Resource Filters
----------------

Filters vs. Middleware
----------------------


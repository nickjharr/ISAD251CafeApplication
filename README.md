# ISAD251CafeApplication

ASP.NET Core MVC Application for a Cafe. ISAD251 Coursework 2019.

This project is implentation of an minimum viable product (MVP) for use by a coffee shop/cafe. 

Youtube Link: https://youtu.be/NL_jtmbIDNU 

**Please Note** two browsers used in video as evidence that it does work cross-browser.

A customer arrives at the homepage - they then navigate to the menu and are able to add items of their choosing to a basket. Once all desired items are added to the basket, the customer checks out. During checkout the customer must provide a table number.

Once this checkout process is complete, the admin live screen (designed to be displayed in the kitchen/behind the counter) will update automatically showing the latest order(s) - complete orders can be dismissed as complete. At this stage, the customer is directed to their order history powered by cookies. Should cookies be disabled, a manual search by order number feature is provided.

As well as viewing orders as they arrive, admins can view a history of orders in all states. Items (products) of all states can be viewed, editted and added/removed from sale from the products page which can be accessed from the main navigation bar. 

The admin portal can be initially accessed from /admin.           (http://cent-5-534.uopnet.plymouth.ac.uk/ISAD251/njharrington/admin)

***API ENDPOINTS***

Get all items:    /api/Items                  (http://cent-5-534.uopnet.plymouth.ac.uk/ISAD251/njharrington/api/Items)

*retreives a list of items for sale, can implement menu easily elsewhere*

Get Open Orders:  /api/Orders/GetOpenOrders   (http://cent-5-534.uopnet.plymouth.ac.uk/ISAD251/njharrington/api/Orders/GetOpenOrders)

*retreives all open orders, allows admin live orders screen to update asynchronously with AJAX/jquery - meaning no need to refresh to see new orders as they come in. Also implements a check to ensure that there are new items before updating page, or you could see annoying flickers*

Post Order Complete /api/Orders/OrderComplete (http://cent-5-534.uopnet.plymouth.ac.uk/ISAD251/njharrington/api/Orders/OrderComplete)

*post the new status of the order to the endpoint to dismiss as complete*

***ROOM FOR IMPROVEMENT***

Triggers were implemented to manage stock levels of products based upon orders, however this caused a problem when orders got larger. Given more time, I would investigate this cause - and how to resolve.

***THIRD PARTY SOFTWARE***

The bootstrap framework was used to manage the styling, while jquery was used to power some client side features, including the live admin panel.

A third party package from NewtonSoft was used for managing serialisation and deserialisation of JSON strings.

Entity Framework was used to bridge the application to the database.

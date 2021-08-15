(function ($) {
    "use strict";

    $("#load").hide();
    $("#buttons").css("display", "none");

    const img_person_click = new Image();
    img_person_click.src = `./images/person_click.png`;

    const img_person_focus = new Image();
    img_person_focus.src = `./images/person_focus.png`;

    const img_person_release = new Image();
    img_person_release.src = `./images/person_release.png`;

    const img_organization_click = new Image();
    img_organization_click.src = `./images/organization_click.png`;

    const img_organization_focus = new Image();
    img_organization_focus.src = `./images/organization_focus.png`;

    const img_organization_release = new Image();
    img_organization_release.src = `./images/organization_release.png`;


    const Size = 10;
    var ID=0;
    var server_url="http://1.15.37.186:9000/";
    var local_docker_url="http://localhost:9000/";
    var local_url="https://localhost:5001/";
    var url=local_docker_url;
    const gData = {
        nodes: [],
        links: []
    };

    // cross-link node objects
    gData.links.forEach(link => {
        const a = gData.nodes[link.source];
        const b = gData.nodes[link.target];
        !a.neighbors && (a.neighbors = []);
        !b.neighbors && (b.neighbors = []);
        a.neighbors.push(b);
        b.neighbors.push(a);

        !a.links && (a.links = []);
        !b.links && (b.links = []);
        a.links.push(link);
        b.links.push(link);
    });

    const elem = document.getElementById('graph');
    var clickNode = null;
    var clickLink = null;
    var hoverNode = null;
    var hoverLink = null;
    const highlightNodes = new Set();
    const highlightLinks = new Set();

    console.log(gData)


    const Graph = ForceGraph()(elem)
        .cooldownTicks(100)
        .graphData(gData)
        .width([700])
        .height([800])

        //Hover Node
        .onNodeHover(node => {
            highlightNodes.clear();
            highlightLinks.clear();
            hoverNode=null;
            hoverLink=null;
            if (node) {
                highlightNodes.add(node);
                hoverNode=node;
                if (node.neighbors !== undefined) {
                    node.neighbors.forEach(neighbor => highlightNodes.add(neighbor));
                    node.links.forEach(link => highlightLinks.add(link));
                }
            }
            change_detail();

        })

        //Click Node
        .onNodeClick(node => {
            if(node===clickNode){
                clickLink = null;
                clickNode=null;
                $("#buttons").css("display","none");
                return;
            }
            highlightNodes.clear();
            highlightLinks.clear();
            clickNode = null;
            clickLink=null;
            if (node) {
                clickNode = node;
                $("#buttons").css("display","");
            }
            change_detail();
            Graph.centerAt(node.x, node.y, 1000);
            Graph.zoom(8, 2000);


        })

        //Click Link
        .onLinkClick(link => {
            if(link===clickLink){
                clickLink = null;
                clickNode=null;
                return;
            }
            highlightNodes.clear();
            highlightLinks.clear();
            clickLink = null;
            clickNode=null;
            if (link) {
                clickLink = link;
            }
            change_detail();
            Graph.centerAt(link.x, link.y, 1000);
            Graph.zoom(8, 2000);
        })

        //Hover Link
        .onLinkHover(link => {
            highlightNodes.clear();
            highlightLinks.clear();
            hoverNode=null;
            hoverLink=null;
            if (link) {
                hoverLink=link;
                highlightLinks.add(link);
                highlightNodes.add(link.source);
                highlightNodes.add(link.target);
            }
            change_detail();
        })

        .autoPauseRedraw(false) // keep redrawing after engine has stopped

        //Link Change Mode
        .linkColor(link => {
            if(clickLink===link){
                return "#d40221";
            }
            else if (highlightLinks.has(link)) {
                return "#57a1e2";
            }
            else return "#a0c6ec";

        })
        .linkWidth(link => highlightLinks.has(link) ? Size / 4 : Size / 6)

        //Node Change Mode
        .nodeCanvasObject((node, ctx) => {
            if(clickNode===node){
                if (node.label === "Organization") {
                    node.img = img_organization_click;
                } else {
                    node.img = img_person_click;
                }
                const size = Size;
                ctx.drawImage(node.img, node.x - size / 2, node.y - size / 2, size, size);
            }
            else if (highlightNodes.has(node)) {
                if (node.label === "Organization") {
                    node.img = img_organization_focus;
                } else {
                    node.img = img_person_focus;
                }
                const size = Size;
                ctx.drawImage(node.img, node.x - size / 2, node.y - size / 2, size, size);
            }
            else {
                if (node.label === "Organization") {
                    node.img = img_organization_release;
                } else {
                    node.img = img_person_release;
                }
                const size = Size;
                ctx.drawImage(node.img, node.x - size / 2, node.y - size / 2, size, size);
            }
        })
        .nodePointerAreaPaint((node, color, ctx) => {
            const size = Size;
            ctx.fillStyle = color;
            ctx.fillRect(node.x - size / 2, node.y - size / 2, size, size); // draw square as pointer trap
        });


    //Others
    Graph.onNodeDragEnd(node => {
        node.fx = node.x;
        node.fy = node.y;
        Graph.zoomToFit(600);
    });

    Graph.onBackgroundClick((event) => {
        Graph.zoomToFit(600);
    });
    Graph.d3Force("x", d3.forceX());
    Graph.d3Force("y", d3.forceY());


    //Search

    $("#input_node_name").on("keypress", function (event) {
        if (event.keyCode == 13) {
            $(this).focus();
            var node_name = $(this).val();
            var node_type = $("#node_type").val();
            var node_limit = $("#node_limit").val()
            node_name = node_name.trim();
            var basic_url=url+"search/";
            if (node_type=="Organization"){
                basic_url+="organization";
            }
            else{
                basic_url+="person";
            }
            if (node_name && node_name != "") {
                $.ajax({
                    url: basic_url,
                    type: "GET",
                    data:{"name":node_name,
                        "limit":node_limit},
                    dataType: "json",
                    success: function (data) {
                        toastr.success(" Find Nodes Successfully ~");
                        console.log(data);
                        var Nodes=null;
                        if(node_type=="Organization"){
                            Nodes=data["organizations"];
                        }
                        else{
                            Nodes=data["people"];
                        }
                        for(var node in Nodes){
                            if(node_type=="Organization"){
                                Nodes[node]["img"]=img_organization_release;
                            }
                            else{
                                Nodes[node]["img"]=img_person_release;
                            }
                            Nodes[node]["id"]=ID;
                            ID+=1;
                        }
                        console.log(Nodes);

                        const { nodes, links } = Graph.graphData();
                        Graph.graphData({
                            nodes: Nodes,
                            links: []
                        });
                    },
                    error: function () {
                        toastr.error("Error , Please Try again !")
                    }
                });
            } else {
                toastr.error("Node Name can't be empty !");
                $("#input_node_name").focus();
            }

        }
    });


    function change_detail() {
        $("#title").text("Detail");
        $("#details").css("display", "none");
        var link=null;
        var node=null;
        if(hoverNode){
            node=hoverNode;
        }else if(hoverLink){
            link=hoverLink;
        }else if(clickNode){
            node=clickNode;
        }else if(clickLink){
            link=clickLink;
        }
        if(node){
            $("#details").css("display", "");
            if(node.label==="Organization"){
                $("#title").text("Organization");
                $("#d1").css("display", "");
                $("#d2").css("display", "");
                $("#d3").css("display", "");
                $("#d4").css("display", "");
                $("#d5").css("display", "");
                $("#d1 .ll").text("Organization Name:");
                $("#d1 .rr").text(node.organizationName);
                $("#d2 .ll").text("Organization Founded Date:");
                $("#d2 .rr").text(node.organizationFoundedDate);
                $("#d3 .ll").text("Score:");
                $("#d3 .rr").text(node.score);
                $("#d4 .ll").text("Registered Address:");
                $("#d4 .rr").text(node.registeredAddress);
                $("#d5 .ll").text("More Information");
                $("#d5 .rr").text(node.url);
                $("#d5 .rr").attr("href",node.url);
            }
            else{
                $("#title").text("Person");
                $("#d1").css("display", "");
                $("#d2").css("display", "");
                $("#d3").css("display", "");
                $("#d4").css("display", "");
                $("#d5").css("display", "");
                $("#d1 .ll").text("Family Name:");
                $("#d1 .rr").text(node.familyName);
                $("#d2 .ll").text("Given Name:");
                $("#d2 .rr").text(node.givenName);
                $("#d3 .ll").text("Honorific:");
                $("#d3 .rr").text(node.honorific);
                $("#d4 .ll").text("Score:");
                $("#d4 .rr").text(node.score);
                $("#d5 .ll").text("More Information");
                $("#d5 .rr").text(node.url);
                $("#d5 .rr").attr("href",node.url);

            }
        }
        else if(link){
            $("#details").css("display", "");
            $("#title").text("Officership");
            $("#d1").css("display", "");
            $("#d2").css("display", "");
            $("#d3").css("display", "");
            $("#d4").css("display", "none");
            $("#d5").css("display", "none");
            $("#d1 .ll").text("Organization:");
            $("#d1 .rr").text(link.target.organizationName);
            $("#d2 .ll").text("Officership:");
            $("#d2 .rr").text(link.officerRole);
            $("#d3 .ll").text("Person:");
            $("#d3 .rr").text(link.source.familyName+" "+link.source.givenName);
        }

    }

    // remove node and links
    function removeNode(node) {
        let { nodes, links } = Graph.graphData();
        links = links.filter(l => l.source !== node && l.target !== node); // Remove links attached to node
        nodes=nodes.filter(n=>n!==node); // Remove node
        nodes.forEach((n, idx) => { n.ID = idx; }); // Reset node ids to array index
        Graph.graphData({ nodes, links });
    }

    //Button Callbacks

    $("#unlock").click(function (){
        toastr.success("node unlock");
        if(clickLink){
            clickLink.fx=null;
            clickLink.fy=null;
            clickLink=null;
        }
        if(clickNode){
            clickNode.fx=null;
            clickNode.fy=null;
            clickNode=null;
        }
        $("#buttons").css("display","none");
    });

    $("#remove").click(function (){
        if(clickNode) {
            removeNode(clickNode);
        }
        clickNode=null;
        change_detail();
        $("#buttons").css("display","none");
        toastr.success("node removed");

    });

    $("#span").click(function (){
        if(clickNode) {
            var basic_url=url+"span/";
            if(clickNode.label==="Organization"){
                basic_url+="organization";
            }
            else{
                basic_url+="person";
            }
            $.ajax({
                url: basic_url,
                type: "GET",
                data:{"id":clickNode.id_neo4j},
                dataType: "json",
                success: function (data) {
                    toastr.success(" Node Spanned Successfully ~");
                    var Nodes=null;
                    var Links=null;
                    if(clickNode.label==="Organization"){
                        Nodes=data["people"];
                    }
                    else{
                        Nodes=data["organizations"];
                    }
                    Links=data["officerships"];
                    console.log(Nodes);
                    console.log(Links);
                    let { nodes, links } = Graph.graphData();
                    for (var i in Nodes){
                        var id=Nodes[i].id_neo4j;
                        var isExist=false;
                        for( var j in nodes){
                            if(nodes[j].id_neo4j===id){
                                isExist=true;
                                break;
                            }
                        }
                        if(isExist){
                            continue;
                        }
                        if(clickNode.label==="Organization"){
                            Nodes[i]["img"]=img_person_release;
                        }
                        else{
                            Nodes[i]["img"]=img_organization_release;
                        }
                        Nodes[i]["id"]=ID;
                        ID+=1;
                        nodes=[...nodes, Nodes[i]];
                    }
                    for(var i in Links){
                        var person_id=Links[i].personId;
                        var organization_id=Links[i].organizationId;
                        var source=null;
                        var target=null;
                        for(var j in nodes){
                            if(person_id===nodes[j].id_neo4j){
                                source=nodes[j].id;
                            }
                            else if(organization_id===nodes[j].id_neo4j){
                                target=nodes[j].id;
                            }
                        }
                        if(source!==null&&target!==null){
                            Links[i]["source"]=source;
                            Links[i]["target"]=target;
                            links=[...links,Links[i]];
                        }
                    }
                    Graph.graphData({
                        nodes: nodes,
                        links: links
                    });
                    clickNode=null;
                    console.log(nodes);
                    console.log(links);

                },
                error: function () {
                    toastr.error("Error , Please Try again !")
                    clickNode=null;
                }
            });
        }
        change_detail();
        $("#buttons").css("display","none");
        toastr.success("node spanned");

    });


})(jQuery);

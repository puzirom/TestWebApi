var tokenKey = "tokenKey";
var userId = "userId";
var sessionId = "sessionId";

$(function () {
    $("#loginBlock").show();
    var serviceHost = "http://service.test.local";

    $("#btnLogin").click(function (e) {
        e.preventDefault();
        var loginData = {
            grant_type: "password",
            username: $("#txtUserName").val(),
            password: $("#txtUserPassword").val()
        };

        $.ajax({
            type: "POST",
            url: serviceHost + "/Token",
            data: loginData
        }).success(function(data) {
            $("#userName").text(data.userName);
            $("#userInfo").css("display", "block");
            $("#loginBlock").css("display", "none");
            sessionStorage.setItem(tokenKey, data.access_token);
            sessionStorage.setItem(userId, data.userId);
            console.log(data);
        }).fail(function(result) {
            console.log(result);
        });
    });

    $("#logOut").click(function (e) {
        e.preventDefault();
        sessionStorage.removeItem(tokenKey);
        sessionStorage.removeItem(userId);
        $("#userInfo").css("display", "none");
        $("#loginBlock").css("display", "block");
    });
    
    $("#btnCollections").click(function (e) {
        e.preventDefault();
        var data = null;
        if ($("#txtPageNumberCol").val() !== "" && $("#txtPageSizeCol").val() !== "")
            data = { pageNumber: $("#txtPageNumberCol").val(), pageSize: $("#txtPageSizeCol").val() }
        $.ajax({
            type: "GET",
            data: data,
            url: serviceHost + "/api/game/getCollections"
        }).success(function (data) {
            console.log(data);
        }).fail(function (result) {
            console.log(result);
        });
    });

    $("#btnCollection").click(function (e) {
        e.preventDefault();
        $.ajax({
            type: "GET",
            data: { id: $("#txtCollectionId").val() },
            url: serviceHost + "/api/game/getCollection"
        }).success(function (data) {
            console.log(data);
        }).fail(function (result) {
            console.log(result);
        });
    });

    $("#btnGames").click(function (e) {
        e.preventDefault();
        var data = null;
        if ($("#txtPageNumber").val() !== "" && $("#txtPageSize").val() !== "")
            data = { pageNumber: $("#txtPageNumber").val(), pageSize: $("#txtPageSize").val() }
        $.ajax({
            type: "GET",
            data: data,
            url: serviceHost + "/api/game/getGames"
        }).success(function (data) {
            console.log(data);
        }).fail(function (result) {
            console.log(result);
        });
    });

    $("#btnGame").click(function (e) {
        e.preventDefault();
        $.ajax({
            type: "GET",
            data: { id: $("#txtGameId").val() },
            url: serviceHost + "/api/game/getGame"
        }).success(function (data) {
            console.log(data);
        }).fail(function (result) {
            console.log(result);
        });
    });

    $("#gameStart").click(function (e) {
        e.preventDefault();
        $.ajax({
            type: "POST",
            url: serviceHost + "/api/game/StartGameSession",
            beforeSend: addTokenKey,
            data: { customerId: sessionStorage.getItem(userId), gameId: $("#txtGameId").val() }
        }).success(function (data) {
            sessionStorage.setItem(sessionId, data.SessionId);
            console.log(data);
        }).fail(function (result) {
            console.log(result);
        });
    });

    $("#gameStop").click(function (e) {
        e.preventDefault();
        $.ajax({
            type: "GET",
            url: serviceHost + "/api/game/StopGameSession",
            beforeSend: addTokenKey,
            data: { sessionId: sessionStorage.getItem(sessionId) }
        }).success(function (data) {
            sessionStorage.removeItem(sessionId);
            console.log(data);
        }).fail(function (result) {
            console.log(result);
        });
    });
});

function addTokenKey(xhr) {
    var token = sessionStorage.getItem(tokenKey);
    xhr.setRequestHeader("Authorization", "Bearer " + token);
}

function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}
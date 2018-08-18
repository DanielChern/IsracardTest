var module = angular.module('mainPage', []);

module.controller('mainPageCtrl', function ($scope, $http) {
    $scope.currentFilter = {};
    $scope.gitHubRows = [];
    $scope.showHolder = false;
    $scope.isBookmark = false;
    $scope.initPage = function () {
        $scope.holderContext = "Plese input some text to input for search";
    }

    $scope.findRepositories = function () {
        $scope.changePage(1, $scope.searchWord)
    }

    $scope.changePage = function (page, filter) {
        $scope.loading = true;
        $http({
            method: 'GET',
            url: 'https://api.github.com/search/repositories?page=' + page + '&q=' + filter + '+in:name'
        }).then(function successCallback(response) {
            $scope.isBookmark = false;
            pagesCount = parseInt(response.data.total_count / 30 , 10) + (response.data.total_count%30 != 0 ? 1 :0);
            currentPage = page;
            $scope.currentFilter = { PagesCount: pagesCount, CurrentPage: currentPage, FilterName: filter, ElementsCount: response.data.total_count };
            $scope.gitHubRows = rebuildResults(response.data.items);
            $scope.loading = false;
        })
    }

    // Get bookmark by Id
    var bindObjectById = function (id) {

    }

    var rebuildResults = function (objects) {
        var result = [];
        objects.forEach(function (node , index) {
            var element = {};
            element["Image"] = node.owner.avatar_url;
            element["Name"] = node.name;
            element["Id"] = node.id;
            result.push(element);
        })
        return result;
    }

    $scope.nextPrevPage = function (type) {
        switch (type) {
            case "next":
                $scope.changePage($scope.currentFilter.CurrentPage + 1 , $scope.currentFilter.FilterName)
                break;
            case "prev":
                $scope.changePage($scope.currentFilter.CurrentPage - 1, $scope.currentFilter.FilterName)
                break;
        }
    }
    $scope.getAllBookmarks = function () {
        $scope.loading = true;
        $http({
            method: 'GET',
            url: '/api/Bookmarks/GetAllBookmarks'
        }).then(function successCallback(response) {
            $scope.holderContext = response.data.Text;
            $scope.currentFilter = { ElementsCount: 0 };
            $scope.gitHubRows = [];
            if (response.data.Status) {
                $scope.isBookmark = true;
                $scope.gitHubRows = response.data.Content.Bookmarks;
                showHolder = false;
                $scope.currentFilter = { ElementsCount : 0};
            } else {
                showHolder = true;
            }
            $scope.loading = false;
        })
    }


    $scope.addToBookmatks = function (element, node) {
        var bookmark = {};
        bookmark.Id = element.Id;
        bookmark.Name = element.Name;
        bookmark.Image = element.Image;
        $http({
            url: "/api/Bookmarks/AddToBookmarks",
            method: 'POST',
            headers: {  
                "Content-Type": "application/json"  
            }  ,
            data: JSON.stringify({ 'bookmark': bookmark })
        }).then(function successCallback(response) {
            if (response.data.Status) {
                toastr.success("Bookmark was added.")
            } else {
                if (response.data.StatusType == 2) {
                    toastr.warning("Bookmark already added.")
                } else {
                    toastr.error(response.data.Text)
                }
            }
        })
    }
})


module.filter('cut', function () {
    return function (value, wordwise, max, tail) {
        if (!value) return '';

        max = parseInt(max, 10);
        if (!max) return value;
        if (value.length <= max) return value;

        value = value.substr(0, max);
        if (wordwise) {
            var lastspace = value.lastIndexOf(' ');
            if (lastspace !== -1) {
                //Also remove . and , so its gives a cleaner result.
                if (value.charAt(lastspace - 1) === '.' || value.charAt(lastspace - 1) === ',') {
                    lastspace = lastspace - 1;
                }
                value = value.substr(0, lastspace);
            }
        }

        return value + (tail || ' …');
    };
});
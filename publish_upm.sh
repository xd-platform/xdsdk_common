#!/bin/sh
# 获取当前分支
currentBranch=$(git symbolic-ref --short -q HEAD)

upmModule=("xd-sdk-account-upm" "xd-sdk-common-upm" "xd-sdk-payment-upm")
module=("Account" "Common" "Payment")
githubRepoName=("xd_sdk_account_upm" "xd_sdk_common_upm" "xd_sdk_payment_upm")

#单发模块
#upmModule=("xd-sdk-common-upm")
#module=("Common")
#githubRepoName=("xd_sdk_common_upm")

tag=$1
#是否正式发布，测试发布时发布到RoongfLee的个人目录下
publish2Release=$2

# 发布 UPM 脚本
publishUPM() {
    git tag -d $(git tag)
    
    git branch -D github_upm
    
    git subtree split --prefix=Assets/XD/SDK/"$1" --branch github_upm
    
    git remote rm "$2"
    
    if [ $publish2Release = true ]; then
        echo "start push $2 to git@github.com:xd-platform/$4.git"
        git remote add "$2" git@github.com:xd-platform/"$4".git
    else
        echo "start push $2 to git@github.com:suguiming/$4.git"  
        git remote add "$2" git@github.com:suguiming/"$4".git
    fi;
    
    git checkout github_upm --force
    
    git tag "$3"
    
    git fetch --unshallow github_upm
    
    git push "$2" github_upm --force --tags
    
    git checkout "$currentBranch" --force
    
#    if [ $publish2Release ]; then
#        gh release create "$3" XD-SDK-UnityPackage/XD_SDK_"$1"_"$3".unitypackage -t "$3" -F ./Assets/XD/SDK/"$1"/VERSION_LOG.md -d -R https://github.com/RoongfLee/"$4"-Unity
#    else
#        gh release create "$3" XD-SDK-UnityPackage/XD_SDK_"$1"_"$3".unitypackage -t "$3" -F ./Assets/XD/SDK/"$1"/VERSION_LOG.md -d -R https://github.com/TapTap/"$4"-Unity
#    fi;
    
}
for ((i=0;i<${#upmModule[@]};i++)); do
    publishUPM "${module[$i]}" "${upmModule[$i]}" "$tag" "${githubRepoName[$i]}" 
done
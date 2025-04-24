//UNITY_SHADER_NO_UPGRADE
#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED

float DistanceBetween_float(float3 player, float3 object)
{
    return sqrt((player.x - object.x) * (player.x - object.x) + (player.z - object.z) * (player.z - object.z));

}

void MyFunction_float(float3 position, float3 player1, float3 player2, float3 player3, float3 player4, out float3 Out)
{
    float3 retVal = player1;
    float dist1 = DistanceBetween_float(player1, position);
    float closestDist = dist1;
    float dist2 = DistanceBetween_float(player2, position);
    if (dist2 < closestDist)
    {
        closestDist = dist2;
        retVal = player2;
    }
    
    float dist3 = DistanceBetween_float(player3, position);
    if (dist3 < closestDist)
    {
        closestDist = dist3;
        retVal = player3;
    }
    
    float dist4 = DistanceBetween_float(player4, position);
    if (dist4 < closestDist)
    {
        closestDist = dist4;
        retVal = player4;
    }
    
    Out = retVal;
}
#endif //MYHLSLINCLUDE_INCLUDED